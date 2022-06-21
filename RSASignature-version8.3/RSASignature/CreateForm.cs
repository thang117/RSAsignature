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
    public partial class CreateForm : Form
    {

        #region khai báo biến
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();      // khai báo thư viện RSA
        string pathKeysXMLPrivate = "";                                     //đường dẫn khoá bí mật
        string pathKeysXMLPublic = "";                                  //đường dẫn khoá công khai
        string pathSignatureEnCrypt = "";                         //đường dẫn chữ ký số
        
        String pathKeyRSApublic = "";                         //đường dẫn khoá công khai để giải mã
        #endregion

        #region code về form và event

        //code về form và event
        public CreateForm()
        {
            InitializeComponent();
        }

        //event form1 load lên
        private void Form1_Load(object sender, EventArgs e)
        {
            //tạo option cho combobox độ dài key (tuỳ chọn xổ xuống)
            comboBoxLengKey.Items.Add("512 bits");
            comboBoxLengKey.Items.Add("1024 bits");
            comboBoxLengKey.Items.Add("2048 bits");
            comboBoxLengKey.Items.Add("4096 bits");
            comboBoxLengKey.Text = "1024 bits";         //mặc định là 1024 bits
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
            comboBoxLengKey.Text = "1024 bits";
            tbPrivateKey.Clear();
            tbPublicKey.Clear();
            tbKetQuaBam1.Clear();
            tbInput1.Clear();
            tbChuKy.Clear();
            enabledOrDisableButtons(true);

           

            pathKeysXMLPrivate = "";
            pathKeysXMLPublic = "";
            pathSignatureEnCrypt = "";
            RSA.Clear();
            
            xoaHash("hash.txt");
            RSA = new RSACryptoServiceProvider();
            


        }

        private void enabledOrDisableButtons(bool isEnable)                         //option mở hay khoá các nút và textbox
        {
            this.btBam.Enabled = isEnable;      
            this.btImportFile1.Enabled = isEnable;         
            this.btTaoChuKy.Enabled = isEnable;
            this.btTaoKhoa.Enabled = isEnable;

            comboBoxLengKey.Enabled = isEnable;
            tbPrivateKey.Enabled = isEnable;
            tbPublicKey.Enabled = isEnable;
            tbKetQuaBam1.Enabled = isEnable;
            tbInput1.Enabled = isEnable;
            tbChuKy.Enabled = isEnable;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            Verification f2 = new Verification();
            this.Visible = false;
            f2.ShowDialog();
            this.Close();
            xoaHash("hash.txt");
        }

        private void xoaHash(string filePath)                         //hàm xoá file hash lưu trữ
        {
            if (File.Exists(filePath))
            {
                // Xóa file
                System.IO.File.WriteAllText(filePath, "");
            }
        }
        #endregion

        #region code tạo khoá RSA

        private delegate void btnEncryptDecrypt();      //khởi tạo hàm tạo khoá RSA
        private void btTaoKhoa_Click(object sender, EventArgs e)                         //event khi nút tạo khoá đc click
        {


            #region Tạo file lưu khoá bí mật
            // Tạo file chứa khoá bí mật
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            MessageBox.Show("Please choose location to store the private key!");
            saveFileDialog1.DefaultExt = "xml";
            saveFileDialog1.Filter = "Xml File|*.xml|All File|*.*";
            saveFileDialog1.Title = "Chọn nơi lưu khoá bí mật";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            saveFileDialog1.RestoreDirectory = true;
            String pathKeyRSAPrivate = saveFileDialog1.FileName;   //pathkeyRSAPrivate giữ đường dẫn file khoá bí mật
            #endregion

            #region Tạo file lưu khoá công khai
            //tạo file chứa khoá công khai
            SaveFileDialog saveFileDialog2 = new SaveFileDialog();
            MessageBox.Show("Please choose location to store the public key!");
            saveFileDialog2.DefaultExt = "xml";
            saveFileDialog2.Filter = "Xml File|*.xml|All File|*.*";
            saveFileDialog2.Title = "Chọn nơi lưu khoá công khai";
            if (saveFileDialog2.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            saveFileDialog2.RestoreDirectory = true;
            pathKeyRSApublic = saveFileDialog2.FileName; //pathKeyRSApublic giữ đường dẫn file khoá công khai
            #endregion


            #region Tạo khoá bằng thư viện RSA
            //lấy giá trị độ dài key
            int lengthKey = 0;
            if (this.comboBoxLengKey.Text == "1024 bits") lengthKey = 1024;
            else if (this.comboBoxLengKey.Text == "512 bits") lengthKey = 512;
            else if (this.comboBoxLengKey.Text == "2048 bits") lengthKey = 2048;
            else if (this.comboBoxLengKey.Text == "4096 bits") lengthKey = 4096;

            //tạo key có độ dài lengthKey
            RSA = new RSACryptoServiceProvider(lengthKey);

            // Lưu khoá bí mật vào đường dẫn pathKeyRSA
            File.WriteAllText(pathKeyRSApublic, RSA.ToXmlString(true)); // đối với RSA, mã hoá bằng public key=(RSA.ToXmlString(false)), giải mã bằng private=(RSA.ToXmlString(true))
                                                                        // nhưng đây là chữ ký số mã hoá bằng private, giải mã bằng publicKey nên chức năng của 2 khoá sẽ ngược lại
                                                                        // suy ra chung ta sẽ tạo khoá công khai của RSA (RSA.ToXmlString(flase))  để mã hoá chữ ký
                                                                        // và chúng ta tạo khoá bí mật của RSA (RSA.ToXmlString(true)) để giải mã chữ ký
                                                                        // Lưu khoá bí mật vào đường dẫn pathKeyRSApublic
            File.WriteAllText(pathKeyRSAPrivate, RSA.ToXmlString(false));
            #endregion


            #region Đọc file khoá bí mật và xuất ra ô tbPrivate
            //đọc file xml khoá bí mật
            pathKeysXMLPrivate = pathKeyRSAPrivate;
            if (File.Exists(pathKeysXMLPrivate))
            {
                if (Path.GetExtension(pathKeysXMLPrivate) == ".xml") //kiểm tra định dạng
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(File.ReadAllText(pathKeysXMLPrivate)); //đọc khoá bí mật
                    try
                    {
                        XmlNode xnList = xml.SelectSingleNode("/RSAKeyValue");
                        tbPrivateKey.Text = xnList.InnerText;
                    }
                    catch
                    {
                        MessageBox.Show("Key generation error");
                    }
                }
            }
            #endregion

            #region Đọc file khoá công khai và xuất ra ô tbPublic
            //đọc file xml khoá công khai
            pathKeysXMLPublic = pathKeyRSApublic;
            if (File.Exists(pathKeysXMLPublic))
            {
                if (Path.GetExtension(pathKeysXMLPublic) == ".xml") //kiểm tra định dạng
                {
                    XmlDocument xml = new XmlDocument();
                    xml.LoadXml(File.ReadAllText(pathKeysXMLPublic)); //đọc khoá công khai
                    try
                    {
                        XmlNode xnList = xml.SelectSingleNode("/RSAKeyValue");
                        tbPublicKey.Text = xnList.InnerText;
                    }
                    catch
                    {
                        MessageBox.Show("Key generation error");
                    }
                }
            }
            #endregion

            MessageBox.Show("Create key has length " + lengthKey.ToString() + " bits success.");


        }

        #endregion

        #region code Hash
        public static string SHA256(string path)                         //hàm băm SHA256
        {
            try
            {
                using (FileStream stream = File.OpenRead(path))           //tiến hành băm dữ liệu từ đường dẫn
                {
                    SHA256Managed sha = new SHA256Managed();
                    byte[] hash = sha.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", String.Empty);
                }
            }
            catch
            {
                MessageBox.Show("Path file error!");
                return "";
            }
        }
        

        private void btBam_Click(object sender, EventArgs e)    //sự kiện nút băm click
        {
            xoaHash("hash.txt");                             //gọi hàm xoá file hash để tránh trường hợp đã có sẵn file hash.txt

            if (tbInput1.Text.Length == 0)
            {
                MessageBox.Show("Add data to create signature");
            }
            else
            {
                tbKetQuaBam1.Text = SHA256(tbInput1.Text);  // file hash.txt sẽ mang giá trị sau khi băm
                System.IO.File.WriteAllText("hash.txt", tbKetQuaBam1.Text);
                btImportFile1.Enabled = false;                         //đóng control nút import

            }
        }


        #endregion

        #region code tạo chữ ký số

        private void btTaoChuKy_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbInput1.Text.Length == 0)
                {
                    MessageBox.Show("Please include the file to sign and hash");
                }
                else
                {
                    // chọn nơi lưu chữ ký số
                    FolderBrowserDialog f1 = new FolderBrowserDialog();
                    if (f1.ShowDialog() == DialogResult.OK)
                    {
                        pathSignatureEnCrypt = f1.SelectedPath;        // đường dẫn được lưu vào pathSignatureEnCrypt 
                    }
                    btnEncryptClick();                         //gọi hàm btnEncryptClick

                }
            }
            catch
            {
                MessageBox.Show("Path file error");
            }
        }


        private void btnEncryptClick()
        {

            if (System.IO.File.Exists(pathKeysXMLPrivate) == false || System.IO.File.Exists(pathKeysXMLPublic) == false || this.tbPrivateKey.Text.Length == 0 || this.tbPublicKey.Text.Length == 0)                         //kiểm tra điều kiện
            {
                MessageBox.Show("The key is invalid! Please reset the form and follow the correct procedure!");
                enabledOrDisableButtons(false);
                return;
            }

            try
            {
                if (tbPrivateKey.Text.Length != 0 &&
                tbPublicKey.Text.Length != 0 &&
                tbKetQuaBam1.Text.Length != 0)
                {

                    string inputFileName = "hash.txt",   // chuỗi inputFileName lưu giá trị của chuỗi băm hash
                        outputFileName = "";          // chuỗi outputFileName dùng để tạo đường dẫn              

                   
                    
                        outputFileName = pathSignatureEnCrypt + "\\" + Path.GetFileName(tbInput1.Text) + ".lhde";   //tạo file chữ ký có tên là file_gốc.lhde tạo đường dẫn thư mục lưu chữ ký
                    
                    //get Keys.

                    RSA = new RSACryptoServiceProvider();
                    RSA.FromXmlString(File.ReadAllText(this.pathKeysXMLPrivate));       //chuẩn bị mã hoá sử dụng đường dẫn đến khoá bí mật, dùng hàm đọc all file xml
                 
                    
                        RSA_Algorithm(inputFileName, outputFileName, RSA.ExportParameters(false), true);                         //gọi hàm xử lý RSA
                        MessageBox.Show("Success created digital signature at the path " + pathSignatureEnCrypt);
                        btBam.Enabled = false;
                        tbChuKy.Text = outputFileName;
                    

                    #region nén vào một thư mục

                    //công đoạn nén file
                    // sơ đồ thư mục lưu trữ: vị trí chọn\signature\secret
                    //trong đó:
                    //      vị trí chọn là vị trí mà người dùng muốn lưu file nén, chứa thư mục signature 
                    //      thư mục signature là thư mục được tạo tự động, là thư mục con nằm trong vị trí được chọn, chứa file inputcopy, thư mục secret và secret.zip
                    //      thư mục secret là thư mục được tạo tự động, là thư mục con của signature, cùng cấp với file inputcopy, chứa file khoá công khai copy và file chữ ký copy, đây cũng là thư mục được nén zip có pass


                    if (MessageBox.Show("Do you want to compress it all into one file??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)    //hỏi xem có muốn gom tất cả nén vào một thư mục không
                    {
                        string folderArchive = "";
                        MessageBox.Show("Please select the path to save the zip archive path");         // chọn đường dẫn lưu thư mục nén
                        FolderBrowserDialog f1 = new FolderBrowserDialog();
                        if (f1.ShowDialog() == DialogResult.OK)
                        {
                            folderArchive = f1.SelectedPath;        // đường dẫn lưu thư mục nén
                        }

                        //tạo thư mục con tại vị trí đã chọn để lưu file input, thư mục secret(gồm chữ ký và khoá công khai) và secret.zip
                        // 1. Đường dẫn tới thư mục cần tạo 
                        string path1 = folderArchive + "\\signature\\";

                        // 2.Khai báo một thể hiện của lớp DirectoryInfo
                        DirectoryInfo directory1 = new DirectoryInfo(path1);

                        // Kiểm tra thư mục chưa tồn tại mới sử dụng phương thức tạo
                        if (!directory1.Exists)
                        {
                            // 3.Sử dụng phương thức Create để tạo thư mục.
                            directory1.Create();
                        } 
                        else
                        {
                            directory1.Delete(true);
                            directory1.Create();
                        }    
                        


                        //tạo thư mục con secret trong thư mục signature để lưu file chữ ký và khoá công khai
                        // 1. Đường dẫn tới thư mục cần tạo 
                        string path2 = folderArchive + "\\signature\\secret\\";

                        // 2.Khai báo một thể hiện của lớp DirectoryInfo
                        DirectoryInfo directory2 = new DirectoryInfo(path2);

                        // Kiểm tra thư mục chưa tồn tại mới sử dụng phương thức tạo
                       if (!directory2.Exists)
                        {
                            // 3.Sử dụng phương thức Create để tạo thư mục.
                            directory2.Create();
                        }
                        


                        string duongDanInput = tbInput1.Text;                                                                   //tạo biến lưu đường dẫn đến input để copy
                        string duongDanChuKy = pathSignatureEnCrypt + "\\" + Path.GetFileName(tbInput1.Text) + ".lhde";
                        string duongDanKhoaCongKhai = pathKeysXMLPublic;


                        string tenInput = Path.GetFileName(tbInput1.Text);                                                     //tạo biến lưu tên file input
                        string tenChuKy = Path.GetFileName(tbInput1.Text +".lhde");                                                     //tạo biến lưu tên file input
                        string tenKhoaCongKhai = Path.GetFileName(pathKeysXMLPublic);                                                     //tạo biến lưu tên file input


                        File.Copy(duongDanInput, folderArchive + "\\signature\\" + tenInput ,true);                        //copy file input vào thư mục
                        File.Copy(duongDanChuKy, folderArchive + "\\signature\\secret\\" + tenChuKy, true);       //copy file chữ ký vào thư mục con secret của thư mục đã chọn 
                        File.Copy(duongDanKhoaCongKhai, folderArchive + "\\signature\\secret\\" + tenKhoaCongKhai, true);          //copy file khoá công khai vào thư mục con secret của thư mục đã chọn 


                        string pathFolderinput = folderArchive + "\\signature\\secret";
                        string pathFolderoutput = folderArchive + "\\signature\\secret.zip";
                        if (File.Exists(pathFolderoutput))
                        {
                            File.Delete(pathFolderoutput);
                        }

                        //CreateSample(folderArchive + "\\signature\\secret.zip", "P@ssw0rdC@nnOtbeKn@w4cr3ck", folderArchive + "\\signature\\secret");           // tạo 1 thư mục con secret trong thư mục đã chọn, sau đó nén thư mục secret
                        ZipFile.CreateFromDirectory(pathFolderinput,pathFolderoutput);           // tạo 1 thư mục con secret trong thư mục đã chọn, sau đó nén thư mục secret

                        if (directory2.Exists)
                        {
                            // 3.Sử dụng phương thức Create để tạo thư mục.
                            directory2.Delete(true);
                        }

                        if (MessageBox.Show("successfully compressed the directory at the path" + pathFolderoutput+ " \n Do you want to open file location??", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)    //hỏi xem có muốn mở đường dẫn thư mục đã nén không
                        {
                            
                            var process = new System.Diagnostics.Process();

                            process.StartInfo = new System.Diagnostics.ProcessStartInfo() { UseShellExecute = true, FileName = folderArchive + "\\signature" };

                            process.Start();
                        }
                        
                    }
                    #endregion

                }
                else
                {
                    MessageBox.Show("Not enough data to encryption!");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed: " + ex.Message);
            }

        }

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
            catch (Exception ex)
            {
                MessageBox.Show("Failed: " + ex.Message);
            }
        }

        private void btImportFile1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    tbInput1.Text = dialog.FileName;            //tbInput.Text giữ đường dẫn dẫn tới file cần băm
                    btTaoKhoa.Enabled = false;
                }
            }
        }





        #endregion

        private void CreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            xoaHash("hash.txt");
        }
    }
}


