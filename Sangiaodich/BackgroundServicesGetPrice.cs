using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using PuppeteerSharp;
using Sangiaodich.DBContext;

namespace Sangiaodich
{
    public class BackgroundServicesGetPrice : IDisposable
    {
        public event Action? OngetData;
        public List<string>? LstMCKHTML { get; set; } = new List<string>();
        private IBrowser? Browser { get; set; }
        private IPage? Page { get; set; }

        private async Task LoadPrice(DBSetIdentityDBContext DB)
        {
            if (Page == null) return;

            var GetContent = await Page.WaitForSelectorAsync(@"#\32 0_FO_TF_SQ_tbodyQuote");
            var jsHandle = await GetContent.GetPropertyAsync("innerHTML");
            var innerText = await jsHandle.JsonValueAsync<string>();

            LstMCKHTML?.Clear();
            var ResultTr = Regex.Matches(innerText, @"(?<1><TR[^>]*>\s*<td.*?</tr>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // Sử dụng StringBuilder để lưu trữ tất cả dữ liệu
            StringBuilder allDataBuilder = new StringBuilder();
            
            foreach (Match item in ResultTr)
            {
                var ResultTd = Regex.Matches(item.Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                if (ResultTd.Count > 1)
                {
                    var MCKModel = new Model.MCK
                    {
                        MCKName = Regex.Match(ResultTd[0].Value, @"<a\b[^>]*>(.*?)<\/a>", RegexOptions.IgnoreCase).Groups[1].Value,
                        Name = Regex.Match(ResultTd[0].Value, @"\btitle=[""'](.*?)[""']", RegexOptions.IgnoreCase).Groups[1].Value.Replace("title=", "").Replace("\"", ""),
                        SGD = Regex.Match(ResultTd[1].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase).Groups[1].Value,
                        Price_Tran = (Regex.Match(ResultTd[2].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        Price_San = (Regex.Match(ResultTd[3].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        Price_TC = (Regex.Match(ResultTd[4].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DM_Price3 = (Regex.Match(ResultTd[5].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DM_KL3 = (Regex.Match(ResultTd[6].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DM_Price2 = (Regex.Match(ResultTd[7].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DM_KL2 = (Regex.Match(ResultTd[8].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DM_Price1 = (Regex.Match(ResultTd[9].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DM_KL1 = (Regex.Match(ResultTd[10].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        KL_Price = (Regex.Match(ResultTd[11].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        KL_Updown = (Regex.Match(ResultTd[12].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        KL_KL = (Regex.Match(ResultTd[13].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        KL_KLGD = (Regex.Match(ResultTd[14].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DB_Price1 = (Regex.Match(ResultTd[15].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DB_KL1 = (Regex.Match(ResultTd[16].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DB_Price2 = (Regex.Match(ResultTd[17].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DB_KL2 = (Regex.Match(ResultTd[18].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DB_Price3 = (Regex.Match(ResultTd[19].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        DB_KL3 = (Regex.Match(ResultTd[20].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        Open = (Regex.Match(ResultTd[21].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        High = (Regex.Match(ResultTd[22].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        Low = (Regex.Match(ResultTd[23].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        TB = (Regex.Match(ResultTd[24].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        GDNDTNN_Buy = (Regex.Match(ResultTd[25].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        GDNDTNN_Sell = (Regex.Match(ResultTd[26].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        RoomNN = (Regex.Match(ResultTd[27].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        CL = (Regex.Match(ResultTd[28].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                        KLTT = (Regex.Match(ResultTd[29].Value, @"<td\b[^>]*>(.*?)<\/td>", RegexOptions.IgnoreCase)).Groups[1].Value,
                    };

                    // Kiểm tra và cập nhật hoặc thêm vào cơ sở dữ liệu
                    if (DB.MCK.Any(x => x.Name == MCKModel.Name))
                    {
                        DB.Update(MCKModel);
                    }
                    else
                    {
                        DB.Add(MCKModel);
                       
                    }

                    await DB.SaveChangesAsync();


                    // Thêm dữ liệu MCKModel vào StringBuilder với dấu phân cách "|"
                    allDataBuilder.AppendLine(
                        
                        $"{MCKModel.MCKName}|{MCKModel.Name}|{MCKModel.SGD}|{MCKModel.Price_Tran}|{MCKModel.Price_San}|{MCKModel.Price_TC}|" +
                        $"{MCKModel.DM_Price3}|{MCKModel.DM_KL3}|{MCKModel.DM_Price2}|{MCKModel.DM_KL2}|{MCKModel.DM_Price1}|{MCKModel.DM_KL1}|" +
                        $"{MCKModel.KL_Price}|{MCKModel.KL_Updown}|{MCKModel.KL_KL}|{MCKModel.KL_KLGD}|{MCKModel.DB_Price1}|{MCKModel.DB_KL1}|" +
                        $"{MCKModel.DB_Price2}|{MCKModel.DB_KL2}|{MCKModel.DB_Price3}|{MCKModel.DB_KL3}|{MCKModel.Open}|{MCKModel.High}|" +
                        $"{MCKModel.Low}|{MCKModel.TB}|{MCKModel.GDNDTNN_Buy}|{MCKModel.GDNDTNN_Sell}|{MCKModel.RoomNN}|{MCKModel.CL}|{MCKModel.KLTT}"
                    );

                }
                LstMCKHTML?.Add(item.Value);
            }

            // Tạo tên file với timestamp
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
            string fileName = $"AllData_{timestamp}.txt";

            // Ghi tất cả dữ liệu vào file với tên bao gồm timestamp
            File.WriteAllText(fileName, allDataBuilder.ToString());

            OngetData?.Invoke();
        }

        public async Task ExecuteAsync(DBSetIdentityDBContext DB)
        {
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            Browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = new string[]
                {
                    "--no-sandbox", "--disable-setuid-sandbox", "--disable-dev-shm-usage", "--disable-gpu",
                    "--no-first-run", "--no-zygote", "--single-process", "--disable-background-networking"
                }
            });

            Page = await Browser.NewPageAsync();
            await Page.SetViewportAsync(new ViewPortOptions
            {
                Width = 0,
                Height = 0,
                DeviceScaleFactor = 1
            });

            await Page.GoToAsync("https://priceboard.maybank-kimeng.com.vn/#", new PuppeteerSharp.NavigationOptions { WaitUntil = new[] { WaitUntilNavigation.Networkidle0 } });

            Page.Response += async (s, e) =>
            {
                if (e.Response.Url.Contains("https://priceboard.maybank-kimeng.com.vn/ajaxdata/GetMarketInfo.aspx?rParams=GetMarketInfo"))
                {
                    await LoadPrice(DB);
                }
            };
        }

        public void Dispose()
        {
            Browser?.CloseAsync().Wait();
            GC.SuppressFinalize(this);
        }
    }
}