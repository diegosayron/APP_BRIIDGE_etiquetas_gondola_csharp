using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System.Text.Json;
using APP_BRIIDGE_etiquetas.Api;

namespace APP_BRIIDGE_etiquetas
{
    public partial class Form1 : Form
    {
        private readonly WebView2 browser;

        public Form1()
        {
            InitializeComponent();

            browser = new WebView2();

            browser.Dock = DockStyle.Fill;
            Controls.Add(browser);
            browser.BringToFront();
        }

        private void CoreWebView2_WebMessageReceived(
    object? sender,
    CoreWebView2WebMessageReceivedEventArgs e)
        {
            string json = e.WebMessageAsJson;

            if (!json.Contains("TOKEN_VENDA_ERP"))
                return;

            using JsonDocument doc = JsonDocument.Parse(json);

            string token =
                doc.RootElement.GetProperty("token").GetString()!;

            if (!TokenProvider.Definir(token))
                return;

            Hide();

            FrmEtiqueta frm = new FrmEtiqueta();

            frm.FormClosed += (_, _) => Application.Exit();

            frm.Show();
        }


        private async void Form1_Load(object? sender, EventArgs e)
        {
            string pastaPerfil = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "APP_BRIIDGE_ETIQUETAS");

            var ambiente = await CoreWebView2Environment.CreateAsync(
                userDataFolder: pastaPerfil);

            await browser.EnsureCoreWebView2Async(ambiente);
            browser.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
            browser.CoreWebView2.Settings.IsGeneralAutofillEnabled = true;




            string js = File.ReadAllText(@"Scripts\TokenCapture.js");
            await browser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(js);

            browser.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

            browser.CoreWebView2.Navigate("https://app.briidge.com.br");
        }
    }
}
