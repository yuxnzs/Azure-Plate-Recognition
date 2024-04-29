using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows.Forms;


namespace AzurePlateRecognition {
    public partial class AzurePlateRecognition : Form {
        const string endpoint = "";
        const string key = "";

        const string ocr_endPoint = "";
        const string ocr_key = "";

        public AzurePlateRecognition() {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e) {
            string imgPath;

            if (openFileDialog1.ShowDialog() == DialogResult.OK) {
                imgPath = openFileDialog1.FileName;
                plate.Image = new Bitmap(imgPath); // 開新的圖片放進去

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Prediction-key", key);

                // 將檔案開成 FileStream
                FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(fileStream);
                byte[] buffer = reader.ReadBytes((int)fileStream.Length);
                ByteArrayContent content = new ByteArrayContent(buffer);

                // 將 contentType 的 header 設為 "application/octet-stream"
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // 將圖片資料透過 filestream 傳遞給 endpoint API 並等待其回傳偵測結果
                HttpResponseMessage responseMessage = await client.PostAsync(endpoint, content);

                // 回傳資料內容給 richText
                string jsonStr = await responseMessage.Content.ReadAsStringAsync();

                // 回傳回來的 JSON string Deserialize 變成物件
                CarPlateInfo car_PlateInfo = JsonConvert.DeserializeObject<CarPlateInfo>(jsonStr);
                string message = "";
                // 查看有幾張車牌
                message += $"共辨識到 {car_PlateInfo.predictions.Count} 個車牌\n";

                // 畫筆，框車牌用
                Graphics graphics = plate.CreateGraphics();
                Pen pen = new Pen(Color.Red, 1);
                int pb_Width = plate.Width;
                int pb_Height = plate.Height;

                int count = 0;
                double pro = 0.7; // probability; 設定大於多少才顯示
                int left = 0;
                int top = 0;
                int width = 0;
                int height = 0;

                int adjustedLeft = 0;
                int adjustedTop = 0;
                int adjustedWidth = 0;
                int adjustedHeight = 0;

                foreach (Prediction prediction in car_PlateInfo.predictions) {
                    if (prediction.probability >= pro) {
                        // 原始位置，但不正確，因為在視窗那裡有設定調整圖片 Picture Box 為 StretchImage
                        left = (int)(prediction.boundingBox.left * pb_Width);
                        top = (int)(prediction.boundingBox.top * pb_Height);
                        width = (int)(prediction.boundingBox.width * pb_Width);
                        height = (int)(prediction.boundingBox.height * pb_Height);

                        adjustedLeft = (int)(prediction.boundingBox.left * plate.Image.Width);
                        adjustedTop = (int)(prediction.boundingBox.top * plate.Image.Height);
                        adjustedWidth = (int)(prediction.boundingBox.width * plate.Image.Width);
                        adjustedHeight = (int)(prediction.boundingBox.height * plate.Image.Height);

                        graphics.DrawRectangle(pen, new Rectangle(left, top, width, height));

                        count++;
                        message +=
                            $"Tag Name:".PadRight(15) + $"{prediction.tagName}\n" +
                            $"Tag Id:".PadRight(15) + $"{prediction.tagId}\n" +
                            $"Probability:".PadRight(15) + $"{prediction.probability:p1}\n" +
                            $"Left:".PadRight(15) + $"{left}\n" +
                            $"Top:".PadRight(15) + $"{top}\n" +
                            $"Width:".PadRight(15) + $"{width}\n" +
                            $"Height:".PadRight(15) + $"{height}\n\n";
                    }

                }
                message += $"共有 {count} 個車牌的信心度大於 {pro:p0}";
                fileStream.Close(); // 先關閉                

                // 原本圖片的位置，透過 pictureBox.Image 取得原本的寬度和高度
                int originalImageWidth = plate.Image.Width;
                int originalImageHeight = plate.Image.Height;

                // 裁切並存為新檔案
                Bitmap croppedImage = null;
                try {
                    croppedImage = new Bitmap(adjustedWidth, adjustedHeight);
                } catch (ArgumentException ex) {
                    message = "裁切過程中發生錯誤：" + ex.Message;
                    resultTextBox.Text = message;
                    return; // 確保只有在 croppedImage 被成功初始化後，才會進行後續圖形處理
                }
                
                // 使用 using 為確保實現 IDisposable 介面的物件在使用後被正確地處置，實現自動資源管理機制
                using (Graphics gra = Graphics.FromImage(croppedImage)) {
                    gra.DrawImage(plate.Image,
                        new Rectangle(0, 0, adjustedWidth, adjustedHeight),
                        new Rectangle(adjustedLeft, adjustedTop, adjustedWidth, adjustedHeight),
                        GraphicsUnit.Pixel);
                }

                // 將路徑設定為上三級目錄，然後進入 Cropped_Images 資料夾；從 Debug 到 Cropped_Images 資料夾
                // ..\..\..\Cropped_Images\temp.bmp
                string relativePath = Path.Combine("..", "..", "..", "Cropped_Image", "temp.bmp");
                string directoryPath = Path.GetDirectoryName(relativePath);

                // 檢查資料夾是否存在
                if (!Directory.Exists(directoryPath)) {
                    // 如果資料夾不存在，就自動創建新的資料夾
                    Directory.CreateDirectory(directoryPath);
                }

                croppedImage.Save(relativePath, ImageFormat.Bmp);

                croppedPlate.Image = croppedImage;

                string newImage = relativePath;

                try {
                    // OCR 辨識
                    using (fileStream = new FileStream(newImage, FileMode.Open, FileAccess.Read)) { // 使用 using 確保 FileStream 正確關閉
                        ComputerVisionClient visionClient = new ComputerVisionClient(
                            new ApiKeyServiceClientCredentials(ocr_key),
                            new System.Net.Http.DelegatingHandler[] { }
                        );
                        visionClient.Endpoint = ocr_endPoint;

                        ReadInStreamHeaders textHeaders = await visionClient.ReadInStreamAsync(fileStream);
                        string operationLocation = textHeaders.OperationLocation;
                        Thread.Sleep(3000); // 多等一點時間
                                            // 把後面刪除掉就可以獲得 id；排除最後 36 個不是 operationId 
                        string operationId = operationLocation.Substring(operationLocation.Length - 36);

                        // 取得辨識結果
                        ReadOperationResult result = await visionClient.GetReadResultAsync(Guid.Parse(operationId));
                        // 這裡可能因為 OCR 處理尚未完全完成時嘗試訪問結果，導致 result.AnalyzeResult 為 null，觸發 NullReferenceException
                        IList<ReadResult> textUrlFileResults = null;
                        try {
                            textUrlFileResults = result.AnalyzeResult.ReadResults;
                        } catch (NullReferenceException ex) {
                            message = "OCR 辨識尚未完成，請再試一次：" + ex.Message;
                            resultTextBox.Text = message;
                            return;
                        }
                        
                        string str = "";
                        foreach (ReadResult textResult in textUrlFileResults) {
                            // 取得辨識到的文字
                            foreach (Line line in textResult.Lines) {
                                str += line.Text;
                            }
                        }
                        message += "\n\n車牌號碼：\t" + str;
                    }
                } catch (ComputerVisionOcrErrorException ocrEx) {
                    // 處理特定於 OCR 的異常，如裁切過後的 temp.bmp 太小無法辨識
                    message += "\n\nOCR 辨識過程中發生錯誤：" + ocrEx.Message;
                } catch (Exception ex) {
                    // 處理其他一般異常
                    message += "\n\n辨識過程中發生未知錯誤：" + ex.Message;
                }
                finally {
                    resultTextBox.Text = message;
                }
            }
        }
    }
}