(function () {

    console.log("🔥 HOOK ULTRA-AGRESSIVO DE CAPTURA DE TOKEN INICIADO");

    let tokenCapturado = false;

    function validarToken(token) {
        if (!token || typeof token !== 'string') return false;
        if (token.length < 30 || token.length > 1000) return false;

        // ❌ IGNORAR tokens de reCAPTCHA, Google, Firebase, etc.
        const blacklist = [
            '09AKh', // reCAPTCHA v3
            'AIza',  // Google API Keys
            'ya29',  // Google OAuth
            'FIREBASE',
            'FCM'
        ];

        for (let prefix of blacklist) {
            if (token.startsWith(prefix)) {
                console.log(`⚠️ Token ignorado (blacklist: ${prefix}):`, token.substring(0, 20) + "...");
                return false;
            }
        }

        // ✅ Aceitar tokens que parecem JWT ou tokens aleatórios seguros
        const looksLikeJWT = token.split('.').length === 3;
        const looksLikeRandomToken = /^[a-f0-9]{32,}$/i.test(token);

        return looksLikeJWT || looksLikeRandomToken;
    }

    function enviarToken(token, origem) {
        if (!validarToken(token)) return;
        if (tokenCapturado) return;

        console.log(`✅ TOKEN CAPTURADO via ${origem}:`, token.substring(0, 30) + "...");

        tokenCapturado = true;

        window.chrome.webview.postMessage({
            type: "TOKEN_VENDA_ERP",
            token: token,
            origem: origem
        });
    }

    // ===================================
    // PRIORIDADE 1: INTERCEPTAR HTTP
    // ===================================

    const origSetHeader = XMLHttpRequest.prototype.setRequestHeader;
    XMLHttpRequest.prototype.setRequestHeader = function (name, value) {
        if (name.toLowerCase() === "authorization") {
            const token = value.replace(/Bearer\s+/i, "").trim();
            enviarToken(token, "XMLHttpRequest.setRequestHeader");
        }
        return origSetHeader.apply(this, arguments);
    };

    const origFetch = window.fetch;
    window.fetch = function (...args) {
        const [url, options] = args;

        if (options?.headers) {
            const headers = options.headers;

            if (headers.Authorization || headers.authorization) {
                const authValue = headers.Authorization || headers.authorization;
                const token = authValue.replace(/Bearer\s+/i, "").trim();
                enviarToken(token, "fetch (object headers)");
            }

            if (headers instanceof Headers) {
                const authValue = headers.get('Authorization') || headers.get('authorization');
                if (authValue) {
                    const token = authValue.replace(/Bearer\s+/i, "").trim();
                    enviarToken(token, "fetch (Headers instance)");
                }
            }
        }

        return origFetch.apply(this, args);
    };

    // ===================================
    // PRIORIDADE 2: BUSCAR EM STORAGE (com filtros)
    // ===================================

    function buscarEmStorage() {
        const storages = [
            { name: 'localStorage', storage: window.localStorage },
            { name: 'sessionStorage', storage: window.sessionStorage }
        ];

        storages.forEach(({ name, storage }) => {
            try {
                for (let i = 0; i < storage.length; i++) {
                    const key = storage.key(i);
                    const value = storage.getItem(key);

                    if (!value) continue;

                    // ❌ Ignorar chaves de reCAPTCHA, analytics, etc.
                    if (key.includes('_grecaptcha') ||
                        key.includes('google') ||
                        key.includes('analytics') ||
                        key.includes('firebase')) {
                        continue;
                    }

                    try {
                        const obj = JSON.parse(value);

                        const possiveisChaves = [
                            'token', 'accessToken', 'access_token', 'jwt',
                            'bearerToken', 'authToken', 'auth_token',
                            'Token', 'AccessToken', 'JWT'
                        ];

                        for (let chave of possiveisChaves) {
                            if (obj[chave]) {
                                enviarToken(obj[chave], `${name}.${key}.${chave}`);
                            }
                        }

                        if (obj.data?.token) enviarToken(obj.data.token, `${name}.${key}.data.token`);
                        if (obj.user?.token) enviarToken(obj.user.token, `${name}.${key}.user.token`);
                        if (obj.auth?.token) enviarToken(obj.auth.token, `${name}.${key}.auth.token`);

                    } catch (e) {
                        // Token direto (não JSON)
                        if (value.length > 50 && value.length < 500 && !value.includes(' ') && !value.includes('{')) {
                            enviarToken(value, `${name}.${key} (direct)`);
                        }
                    }
                }
            } catch (e) {
                console.error("Erro ao buscar em", name, e);
            }
        });
    }

    function buscarEmCookies() {
        const cookies = document.cookie.split(';');

        cookies.forEach(cookie => {
            const [name, value] = cookie.trim().split('=');

            if (!value || value.length < 20) return;

            const nomeLower = name.toLowerCase();
            if (nomeLower.includes('token') ||
                nomeLower.includes('auth') ||
                nomeLower.includes('jwt') ||
                nomeLower.includes('bearer')) {

                try {
                    const decoded = decodeURIComponent(value);
                    enviarToken(decoded, `cookie.${name}`);
                } catch (e) {
                    enviarToken(value, `cookie.${name}`);
                }
            }
        });
    }

    function buscarEmGlobais() {
        const caminhos = [
            'window.app', 'window.App', 'window.APP',
            'window.auth', 'window.Auth', 'window.AUTH',
            'window.user', 'window.User', 'window.USER',
            'window.config', 'window.Config', 'window.CONFIG',
            'window.__INITIAL_STATE__'
        ];

        caminhos.forEach(caminho => {
            try {
                const partes = caminho.split('.');
                let obj = window;

                for (let i = 1; i < partes.length; i++) {
                    const parte = partes[i].replace('?', '');
                    obj = obj?.[parte];
                    if (!obj) break;
                }

                if (obj) {
                    const json = JSON.stringify(obj);

                    const patterns = [
                        /"token":"([^"]{30,})"/i,
                        /"accessToken":"([^"]{30,})"/i,
                        /"access_token":"([^"]{30,})"/i,
                        /"jwt":"([^"]{30,})"/i,
                        /"bearerToken":"([^"]{30,})"/i,
                        /"authToken":"([^"]{30,})"/i
                    ];

                    patterns.forEach(pattern => {
                        const match = json.match(pattern);
                        if (match) {
                            enviarToken(match[1], `${caminho} (pattern)`);
                        }
                    });
                }
            } catch (e) { }
        });
    }

    window.addEventListener('storage', (e) => {
        if (e.newValue && e.newValue.length > 50) {
            try {
                const obj = JSON.parse(e.newValue);
                if (obj.token) enviarToken(obj.token, `storage event: ${e.key}`);
            } catch (err) { }
        }
    });

    // ===================================
    // INICIALIZAÇÃO
    // ===================================

    setTimeout(() => {
        if (!tokenCapturado) {
            console.log("🔍 Iniciando busca de token em storage/cookies...");
            buscarEmStorage();
            buscarEmCookies();
            buscarEmGlobais();
        }
    }, 1000);

    setInterval(() => {
        if (!tokenCapturado) {
            buscarEmStorage();
            buscarEmCookies();
            buscarEmGlobais();
        }
    }, 5000);

    console.log("✅ Hook instalado. Aguardando requisições HTTP...");

})();