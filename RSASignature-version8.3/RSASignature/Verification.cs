using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Numerics;
using System.Xml;
using System.Diagnostics;
using System.IO.Compression;

namespace RSASignature
{
    public partial class Verification : Form
    {
        #region khai báo biến
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();      // khai báo thư viện RSA
        
        String pathKeyRSApublic = "";                         //đường dẫn khoá công khai để giải mã
        string pathInput = "";                              // đường dẫn tới file input
        #endregion

        #region code về form và event

        //code về form và event
        public Verification()
        {
            InitializeComponent();
        }

        //event form1 load lên



        //event form1 đóng
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you want close?", "Notification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }


        //event khi chọn menu
        private void MenuQuyTrinhTạoChuKy_Click(object sender, EventArgs e)
        {
            QuyTrinh quytrinhform = new QuyTrinh();                  //show ảnh quy trình tạo chữ ký
            quytrinhform.Show();
        }


        //show danh sách thành viên
        private void MenuThongTinDoAn_Click(object sender, EventArgs e)
        {
            Information informationFrom1 = new Information();       //show thông tin đồ án
            informationFrom1.Show();
        }

        private void btReset_Click(object sender, EventArgs e)                           //sự kiện nút resetform được click
        {

            tbPublicKey.Clear();

            tbDuongDanChuKy.Clear();

            tbInput2.Clear();

            tbDuongDanChuKyDcGiaMa.Clear();

            enabledOrDisableButtons(true);

            RSA.Clear();
            
            pathInput = "";
            RSA = new RSACryptoServiceProvider();
            //


        }

        private void enabledOrDisableButtons(bool isEnable)                         //option mở hay khoá các nút và textbox
        {

            this.btImportChuKy.Enabled = isEnable;

            this.btImportFile2.Enabled = isEnable;

            this.btXacThuc.Enabled = isEnable;


            tbPublicKey.Enabled = isEnable;

            tbDuongDanChuKy.Enabled = isEnable;
            tbInput2.Enabled = isEnable;

            tbDuongDanChuKyDcGiaMa.Enabled = isEnable;

            btnImportPKey.Enabled = isEnable;

        }
        private void xoaHash(string filePath)                         //hàm xoá file hash lưu trữ
        {
            if (File.Exists(filePath))
            {
                // Xóa file
                File.Delete(filePath);
            }
        }
        #endregion

        #region code Hash
        public static string SHA256(string path)                         //hàm băm SHA256
        {
            try
            {
                using (FileStream stream = File.OpenRead(path))                          //tiến hành băm dữ liệu từ đường dẫn
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] hash = sha.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", String.Empty);
                }
            }
            catch
            {
                MessageBox.Show("File path error!");
                return "";
            }
        }





        #endregion

        #region code RSA


        //hàm sử lý mã hoá RSA
        private void RSA_Algorithm(string inputFile, string outputFile, RSAParameters RSAKeyInfo, bool isEncrypt)
        {
            try
            {
                FileStream fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read); //Đọc file input
                FileStream fsCiperText = new FileStream(outputFile, FileMode.Create, FileAccess.Write); //Tạo file output
                fsCiperText.SetLength(0);
                byte[] bin, encryptedData;
                long rdlen = 0;
                long totlen = fsInput.Length;
                int len;
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                RSA.ImportParameters(RSAKeyInfo); //Nhập thông tin khoá RSA (bao gồm khoá riêng)

                int maxBytesCanEncrypted;
                //RSA chỉ có thể mã hóa các khối dữ liệu ngắn hơn độ dài khóa, chia dữ liệu cho một số khối và sau đó mã hóa từng khối và sau đó hợp nhất chúng
                if (isEncrypt)
                    maxBytesCanEncrypted = ((RSA.KeySize - 384) / 8) + 37;      // + 7: OAEP - Đệm mã hóa bất đối xứng tối ưu
                else
                    maxBytesCanEncrypted = (RSA.KeySize / 8);
                //Read from the input file, then encrypt and write to the output file.
                while (rdlen < totlen)
                {
                    if (totlen - rdlen < maxBytesCanEncrypted)
                    {
                        maxBytesCanEncrypted = (int)(totlen - rdlen);
                    }
                    bin = new byte[maxBytesCanEncrypted];
                    len = fsInput.Read(bin, 0, maxBytesCanEncrypted);

                    if (isEncrypt) encryptedData = RSA.Encrypt(bin, false); //Mã Hoá
                    else encryptedData = RSA.Decrypt(bin, false); //Giải mã

                    fsCiperText.Write(encryptedData, 0, encryptedData.Length);
                    rdlen = rdlen + len;

                }

                fsCiperText.Close(); //save file
                fsInput.Close();

            }
            catch
            {
                MessageBox.Show("Cannot decryption this file signatute!");
            }
        }

        #endregion

        #region giải mã chữ ký và xác thực        
        private void btImportFile2_Click(object sender, EventArgs e) // nhap văn bản
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "ALL File (*.*)|*.*";
            if (op.ShowDialog() == DialogResult.OK)
            {
                tbInput2.Text = Convert.ToString(SHA256(op.FileName));      // băm dữ liệu đầu vào
                pathInput = op.FileName;
            }
           
        }
        #region thêm bằng file nén
        private void btnAddFileZip_Click(object sender, EventArgs e)
        {
            btImportChuKy.Enabled = false;
            btnImportPKey.Enabled = false;
            string extractPath = "", zipPath = "";

            OpenFileDialog op = new OpenFileDialog();
            //op.Filter = "ALL File (*.rar)|*.rar|(*.zip)|*.zip" ;
            if (op.ShowDialog() == DialogResult.OK)
            {
                zipPath = op.FileName;
                extractPath = "secretExtract";
                DirectoryInfo directory1 = new DirectoryInfo(extractPath);

                if (directory1.Exists)
                {
                    directory1.Delete(true);
                }
                directory1.Create();

                #region giải nén
                string inputFileName = "", outputFileName = "";
                try
                {
                    using (ZipArchive archive = ZipFile.OpenRead(zipPath))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            Console.WriteLine("Found: " + entry.FullName);

                            // Tìm kiếm các Entry có đuôi .xml
                            if (entry.FullName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))// tim file khóa.xml
                            {
                                string entryOutputPath = Path.Combine(extractPath, entry.FullName);
                                entry.ExtractToFile(entryOutputPath, true);
                                #region Nap khóa công khai
                                pathKeyRSApublic = entryOutputPath;
                                XmlDocument xml = new XmlDocument();
                                xml.LoadXml(File.ReadAllText(pathKeyRSApublic)); //đọc khoá công khai
                                try
                                {
                                    XmlNode xnList = xml.SelectSingleNode("/RSAKeyValue");
                                    tbPublicKey.Text = xnList.InnerText;
                                }
                                catch
                                {
                                    MessageBox.Show("Error input key");
                                }
                                #endregion
                            }
                            else if (entry.FullName.EndsWith(".lhde", StringComparison.OrdinalIgnoreCase))
                            {
                                string entryOutputPath1 = Path.Combine(extractPath, entry.FullName);
                                entry.ExtractToFile(entryOutputPath1, true);
                                #region thêm chũ ký
                                tbDuongDanChuKy.Text = entryOutputPath1;
                                #endregion
                            }

                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error, the file inserted is not a zip file");
                }
                #region giải mã chữ ký
                if (tbPublicKey.Text.Length != 0)
                {
                    inputFileName = tbDuongDanChuKy.Text;
                    outputFileName = "hash.txt";// vị trí giải mã chữ ký

                    RSA = new RSACryptoServiceProvider();// khởi tao khóa

                    RSA.FromXmlString(File.ReadAllText(this.pathKeyRSApublic));//lấy khóa cong khai

                    RSA_Algorithm(inputFileName, outputFileName, RSA.ExportParameters(true), false);//đem vào hàm mã hóa / giải mã
                    tbDuongDanChuKyDcGiaMa.Text = File.ReadAllText(outputFileName);
                }
                else
                {
                    MessageBox.Show("Not enough data to decryption!");
                }
                #endregion
                #endregion



            }

        }
        #endregion
        private void btImportChuKy_Click(object sender, EventArgs e)
        {

            try
            {
                if (tbInput2.Text.Length != 0 && tbPublicKey.Text.Length != 0)
                {
                    string inputFileName = "", outputFileName = "";
                    OpenFileDialog op = new OpenFileDialog(); //lụa chọn chữ kí để giải mã
                    op.Filter = "ALL File (*.*)|*.*";
                    if (op.ShowDialog() == DialogResult.OK)
                        tbDuongDanChuKy.Text = op.FileName; // hiển thị đường dẫn đến chữ ký
                    inputFileName = tbDuongDanChuKy.Text;
                    if (Path.GetExtension(inputFileName) != ".lhde")
                    {
                        MessageBox.Show("This file is not support to decryption!");

                        return;
                    }

                    outputFileName = "hash.txt";
                    RSA = new RSACryptoServiceProvider();// khởi tao khóa

                    RSA.FromXmlString(File.ReadAllText(this.pathKeyRSApublic));//lấy khóa cong khai

                    RSA_Algorithm(inputFileName, outputFileName, RSA.ExportParameters(true), false);//đem vào hàm mã hóa / giải mã
                    tbDuongDanChuKyDcGiaMa.Text = File.ReadAllText(outputFileName);
                    btImportFile2.Enabled = false;

                }
                else
                {
                    MessageBox.Show("Not enough data to decryption!");
                }
            }
            catch
            {
                MessageBox.Show("Wrong Public Key or Signature, please try again");
                btReset.Enabled = false;
                enabledOrDisableButtons(false);
            }

        }

        #region xác thực chữ ký theo cách thông thường (thêm từng file)
        private void btXacThuc_Click(object sender, EventArgs e)
        {
            if (tbInput2.Text.Length == 0 || tbDuongDanChuKy.Text.Length == 0 || tbDuongDanChuKyDcGiaMa.Text.Length == 0 || tbPublicKey.Text.Length == 0)
            {
                MessageBox.Show("Not enough data to decryption!");
            }
            else
            {
                string s1 = Convert.ToString(tbInput2.Text);
                string s2 = File.ReadAllText("hash.txt");
                if (String.Compare(s1, s2) == 0)
                {
                    if (MessageBox.Show("SUCCESS. SIGNATURE CORRECT,\n ORIGINAL TEXT IS NOT CHANGED!!\n DO YOU WANT OPEN FILE?", "Vertification", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.OK)
                    {
                        var process = new System.Diagnostics.Process();

                        process.StartInfo = new System.Diagnostics.ProcessStartInfo() { UseShellExecute = true, FileName = pathInput };

                        process.Start();

                        btImportChuKy.Enabled = false;
                        btImportFile2.Enabled = false;
                    }

                }
                else
                {
                    MessageBox.Show("FAILURE. WRONG SIGNATURE OR DATA WAS CHANGED CONTENT! \nDANGER IF OPENING FILE!! ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    btImportChuKy.Enabled = false;
                    btImportFile2.Enabled = false;
                }
                DirectoryInfo directory1 = new DirectoryInfo("secretExtract");

                if (directory1.Exists)
                {
                    directory1.Delete(true);
                }
            }

        }
        #endregion

        #region nhap khóa công khai
        private void btnImportPKey_Click(object sender, EventArgs e)
        {

            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "AllFile(*.xml)|*.xml";
            op.Title = "Add public key.";

            if (op.ShowDialog() == DialogResult.OK)
            {
                pathKeyRSApublic = op.FileName;
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(File.ReadAllText(op.FileName)); //đọc khoá công khai
                try
                {
                    XmlNode xnList = xml.SelectSingleNode("/RSAKeyValue");
                    tbPublicKey.Text = xnList.InnerText;
                }
                catch
                {
                    MessageBox.Show("Error input key");
                }

            }
        }





        #endregion

        #endregion

        private void Verification_FormClosed(object sender, FormClosedEventArgs e)
        {
            xoaHash("hash.txt");
        }
    }
}
