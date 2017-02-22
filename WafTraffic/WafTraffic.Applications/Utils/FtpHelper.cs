using System;
using System.Net.FtpClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Threading;
using System.ComponentModel.Composition;
using DotNet.Business;
using System.Windows.Forms;
using DotNet.Utilities;
using System.Windows.Media.Imaging;

namespace WafTraffic.Applications.Utils
{
    [Export]
    public class FtpHelper
    {
        #region Private Fields

        //Constants
        private const string STR_FTP_HOST = "ip";
        //private const string STR_FTP_HOST = "ip";
        private const int INT_FTP_PORT = 21;
        private const string STR_FTP_USER_NAME = "yctrafficftp";
        private const string STR_FTP_PASSWORD = "cncher2015";
        private const string STR_FTP_ROOT_DOCUMENT = "Document";
        private const string STR_FTP_ROOT_PICTURE = "Picture";
        private const string STR_FTP_ROOT_THUMBNAIL = "Thumbnail";
        private const string STR_FTP_ROOT_VERSIONS = "Versions";
        //private const int INT_BUFFER_SIZE = 8192;
        private const int INT_BUFFER_SIZE = 16384;

        private string violationImagePath;
        private string violationPicSubRoot;
        private string violationThumbSubRoot;

        //private static FtpHelper instance = null;
        //private static object locker = new object();
        private FtpClient _ftpc;
        private Account account;
        private bool _reconnecting;
        private X509Certificate2Collection Certificates;
        private System.Threading.Timer tKeepAlive;

        #endregion

        #region Constructor

        public FtpHelper()
        {
            Initialize();
        }

        /*
        public static FtpHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new FtpHelper();
                            instance.Initialize();
                        }
                    }
                }
                return instance;
            }
        }
        */

        #endregion

        #region Public Events

        //public event EventHandler DownloadComplete;
        //public event EventHandler<ConnectionClosedEventArgs> ConnectionClosed;
        //public event EventHandler ReconnectingFailed;
        //public event EventHandler<ValidateCertificateEventArgs> ValidateCertificate;
        //public event EventHandler<TransferProgressArgs> TransferProgress;

        #endregion

        #region Property

        public string ViolationImagePath
        {
            get
            {
                return violationImagePath;
            }
            set
            {
                if (violationImagePath != value)
                {
                    violationImagePath = value;
                }
            }
        }

        public string ViolationPicSubRoot
        {
            get
            {
                return violationPicSubRoot;
            }
            set
            {
                if (violationPicSubRoot != value)
                {
                    violationPicSubRoot = value;
                }
            }
        }

        public string ViolationThumbSubRoot
        {
            get
            {
                return violationThumbSubRoot;
            }
            set
            {
                if (violationThumbSubRoot != value)
                {
                    violationThumbSubRoot = value;
                }
            }
        }

        public string VersionsRoot
        {
            get
            {
                return STR_FTP_ROOT_VERSIONS;
            }
        }

        #endregion


        #region Methods

        public string UploadFile(FtpFileType type, string localPath)
        {
            string file = string.Empty;
            string subRoot;
            Stream stream = null;
            try
            {
                switch (type)
                {
                    case FtpFileType.Picture:
                        subRoot = STR_FTP_ROOT_PICTURE;
                        break;
                    case FtpFileType.Thumbnail:
                        subRoot = STR_FTP_ROOT_THUMBNAIL;
                        break;
                    case FtpFileType.ViolationPicture:
                        subRoot = STR_FTP_ROOT_PICTURE + "/" + violationImagePath;
                        violationPicSubRoot = subRoot;
                        break;
                    case FtpFileType.ViolationThumbnail:
                        subRoot = STR_FTP_ROOT_THUMBNAIL + "/" + violationImagePath;
                        violationThumbSubRoot = subRoot;
                        break;
                    default:
                        subRoot = STR_FTP_ROOT_DOCUMENT;
                        break;
                }
                if (ValidateUtil.IsBlank(localPath))
                {
                    //MessageBox.Show("上传文件路径不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }

                if (!isConnected) Connect();
                string subPath = GetSubDirectory(subRoot);
                Guid guid = Guid.NewGuid();
                file = subPath + "/" + guid;
                if (type == FtpFileType.Thumbnail || type == FtpFileType.ViolationThumbnail)
                {
                    string imageType = localPath.Substring(localPath.LastIndexOf('.')+1);
                    stream = ImageUtil.Instance.MakeThumbnail(localPath, 128, 128, "H", imageType);
                    Upload(stream, file);
                }
                else
                {
                    Upload(localPath, file);
                }
            }
            catch (System.Exception ex)
            {
                file = string.Empty;
                CurrentLoginService.Instance.LogException(ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }

            }

            return file;
        }

        public string UpdateFile(FtpFileType type, string localPath, string oldFile)
        {
            string file = string.Empty;
            string subRoot;
            Stream stream = null;
            try
            {
                switch (type)
                {
                    case FtpFileType.Picture:
                        subRoot = STR_FTP_ROOT_PICTURE;
                        break;
                    case FtpFileType.Thumbnail:
                        subRoot = STR_FTP_ROOT_THUMBNAIL;
                        break;
                    default:
                        subRoot = STR_FTP_ROOT_DOCUMENT;
                        break;
                }

                if (ValidateUtil.IsBlank(localPath))
                {
                    //MessageBox.Show("上传文件路径不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty;
                }

                if (!isConnected) Connect();
                string subPath = GetSubDirectory(subRoot);
                Guid guid = Guid.NewGuid();
                file = subPath + "/" + guid;
                if (type == FtpFileType.Thumbnail)
                {
                    string imageType = localPath.Substring(localPath.LastIndexOf('.'));
                    stream = ImageUtil.Instance.MakeThumbnail(localPath, 128, 128, "H", imageType);
                    Upload(stream, file);
                }
                else
                {
                    Upload(localPath, file);
                }
                if (!ValidateUtil.IsBlank(oldFile))
                {
                    Remove(oldFile);
                }
            }
            catch (System.Exception ex)
            {
                file = string.Empty;
                CurrentLoginService.Instance.LogException(ex);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }

            }

            return file;
        }

        public bool DownloadFile(string fromFile, string toFile)
        {
            bool ret = false;
            try
            {
                if (ValidateUtil.IsBlank(fromFile) || ValidateUtil.IsBlank(toFile))
                {
                    //MessageBox.Show("文件路径不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return ret;
                }
                if (!isConnected) Connect();
                Download(fromFile, toFile);
                ret = true;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return ret;
        }

        public BitmapImage DownloadFile(string fromFile)
        {
            BitmapImage img = null;
            try
            {
                if (ValidateUtil.IsBlank(fromFile))
                {
                    //MessageBox.Show("文件路径不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return img;
                }
                if (!isConnected) Connect();
                Download(fromFile, out img);
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
                img = null;
            }
            return img;
        }

        public bool RemoveFile(string file)
        {
            bool ret = false;
            try
            {
                if (!isConnected) Connect();
                if (!ValidateUtil.IsBlank(file))
                {
                    Remove(file);
                }
                ret = true;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return ret;
        }

        public bool RemoveDirectory(string path)
        {
            bool ret = false;
            try
            {
                if (!isConnected) Connect();
                if (!ValidateUtil.IsBlank(path))
                {
                    RemoveFolder(path);
                }
                ret = true;
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return ret;
        }

        public List<ClientItem> GetAllFiles(string cpath)
        {
            if (!isConnected) Connect();

            if (!Exists(cpath)) MakeFolder(cpath);

            return List(cpath, true).ToList();
        }

        private string GetSubDirectory(string subRoot)
        {
            string tpath = "/" + subRoot;
            try
            {
                DateTime dt = DateTime.Now;
                tpath += "/" + dt.Year.ToString();
                //if (!Exists(tpath)) MakeFolder(tpath);
                tpath += "/" + dt.Month.ToString();
                //if (!Exists(tpath)) MakeFolder(tpath);
                tpath += "/" + dt.Day.ToString();
                if (!Exists(tpath)) MakeFolder(tpath);

            }
            catch (System.Exception ex)
            {

            }
            return tpath;
        }

        private void Initialize()
        {
            try
            {
                this.Certificates = new X509Certificate2Collection();

                account = new Account();
                account.Host = STR_FTP_HOST;
                account.Port = INT_FTP_PORT;
                account.Username = STR_FTP_USER_NAME;
                account.Password = STR_FTP_PASSWORD;
                account.RemotePath = "./";

                _ftpc = new FtpClient { Host = account.Host, Port = account.Port };
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

        }

        /// <summary>
        /// Connect to the remote servers, with the details from Profile
        /// </summary>
        /// <param name="reconnecting">True if this is an attempt to re-establish a closed connection</param>
        private void Connect(bool reconnecting = false)
        {
            if (isConnected) return;

            // Add accepted certificates
            _ftpc.ClientCertificates.AddRange(Certificates);

            _ftpc.Credentials = new NetworkCredential(account.Username, account.Password);

            try
            {
                _ftpc.Connect();
            }
            catch (Exception exc)
            {
                // Since the ClientCertificates are added when accepted in ValidateCertificate, the first 
                // attempt to connect will fail with an AuthenticationException. If this is the case, a 
                // re-connect is attempted, this time with the certificates properly set.
                // This is a workaround to avoid storing Certificate files locally...
                if (exc is System.Security.Authentication.AuthenticationException
                    && _ftpc.ClientCertificates.Count <= 0)
                    Connect();
                else
                    throw;
            }

            account.HomePath = WorkingDirectory;
            account.RemotePath = WorkingDirectory;

            // Periodically send NOOP (KeepAlive) to server if a non-zero interval is set            
            SetKeepAlive();
        }

        /// <summary>
        /// Attempt to reconnect to the server. Called when connection has closed.
        /// </summary>
        private void Reconnect()
        {
            if (_reconnecting) return;
            try
            {
                _reconnecting = true;
                Connect();
            }
            catch (Exception ex)
            {
                //ReconnectingFailed.SafeInvoke(null, EventArgs.Empty);
            }
            finally
            {
                _reconnecting = false;
            }
        }

        /// <summary>
        /// Close connection to server
        /// </summary>
        public void Disconnect()
        {
            _ftpc.Disconnect();
        }

        /// <summary>
        /// Keep the connection to the server alive by sending the NOOP command
        /// </summary>
        private void SendNoOp()
        {
            //if (controller.SyncQueue.Running) return;

            try
            {
                _ftpc.Execute("NOOP");
            }
            catch (Exception ex)
            {
                Reconnect();
            }
        }

        /// <summary>
        /// Set a timer that will periodically send the NOOP
        /// command to the server if a non-zero interval is set
        /// </summary>
        private void SetKeepAlive()
        {
            // Dispose the existing timer
            UnsetKeepAlive();

            if (tKeepAlive == null) tKeepAlive = new System.Threading.Timer(state => SendNoOp());

            if (account.KeepAliveInterval > 0)
                tKeepAlive.Change(1000 * 10, 1000 * account.KeepAliveInterval);
        }

        /// <summary>
        /// Dispose the existing KeepAlive timer
        /// </summary>
        private void UnsetKeepAlive()
        {
            if (tKeepAlive != null) tKeepAlive.Change(0, 0);
        }

        private void Upload(string localpath, string remotepath)
        {
            using (Stream file = File.OpenRead(localpath),
                            rem = _ftpc.OpenWrite(remotepath))
            {
                var buf = new byte[INT_BUFFER_SIZE];
                int read;
                long total = 0;


                while ((read = file.Read(buf, 0, Convert.ToInt32(buf.Length))) > 0)
                {
                    rem.Write(buf, 0, Convert.ToInt32(read));
                    total += read;
                }
                rem.Flush();
                file.Flush();
                rem.Close();
                file.Close();
            }
        }

        private void Upload(Stream localStream, string remotepath)
        {
            using (Stream rem = _ftpc.OpenWrite(remotepath))
            {
                var buf = new byte[INT_BUFFER_SIZE];
                int read;
                long total = 0;

                localStream.Seek(0, SeekOrigin.Begin);
                while ((read = localStream.Read(buf, 0, Convert.ToInt32(buf.Length))) > 0)
                {
                    rem.Write(buf, 0, Convert.ToInt32(read));
                    total += read;
                }
                rem.Flush();
                localStream.Flush();
                rem.Close();
                localStream.Close();
            }
        }

        private void Download(string cpath, string lpath)
        {
            using (Stream file = File.OpenWrite(lpath), rem = _ftpc.OpenRead(cpath))
            {
                var buf = new byte[INT_BUFFER_SIZE];
                int read;

                while ((read = rem.Read(buf, 0, Convert.ToInt32(buf.Length))) > 0)
                    file.Write(buf, 0, Convert.ToInt32(read));

                rem.Flush();
                file.Flush();
                rem.Close();
                file.Close();
            }
        }

        private void Download(string cpath, out BitmapImage img)
        {
            BitmapImage image = null;
            using (Stream rem = _ftpc.OpenRead(cpath))
            {
                var buf = new byte[INT_BUFFER_SIZE];
                int read;

                MemoryStream ms = new MemoryStream();
                ms.Seek(0, SeekOrigin.Begin);
                while ((read = rem.Read(buf, 0, Convert.ToInt32(buf.Length))) > 0)
                    ms.Write(buf, 0, Convert.ToInt32(read));

                rem.Flush();
                rem.Close();

                image = new BitmapImage();
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                image.BeginInit();
                image.StreamSource = ms;
                image.EndInit();
                //ms.Close();
            }
            img = image;
        }

        private void Rename(string oldname, string newname)
        {
            _ftpc.Rename(oldname, newname);
        }

        private void MakeFolder(string cpath)
        {
            try
            {
                _ftpc.CreateDirectory(cpath);
            }
            catch
            {
                if (!Exists(cpath)) throw;
            }
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="cpath">Path to the file</param>
        private void Remove(string cpath)
        {
            _ftpc.DeleteFile(cpath);
        }

        /// <summary>
        /// Delete a remote folder and everything inside it
        /// </summary>
        /// <param name="path">Path to folder that will be deleted</param>
        /// <param name="skipIgnored">if true, files that are normally ignored will not be deleted</param>
        private void RemoveFolder(string path, bool skipIgnored = true)
        {
            if (!Exists(path)) return;

            // Empty the folder before deleting it
            // List is reversed to delete an files before their parent folders
            foreach (var i in ListRecursive(path, skipIgnored).Reverse())
            {
                Console.Write("\r Removing: {0,50}", i.FullPath);
                if (i.Type == ClientItemType.File)
                    Remove(i.FullPath);
                else
                {
                    _ftpc.DeleteDirectory(i.FullPath);
                }
            }

            _ftpc.DeleteDirectory(path);
        }

        /// <summary>
        /// Make sure that our client's working directory is set to the user-selected Remote Path.
        /// If a previous operation failed and the working directory wasn't properly restored, this will prevent further issues.
        /// </summary>
        /// <returns>false if changing to RemotePath fails, true in any other case</returns>
        private bool CheckWorkingDirectory()
        {
            try
            {
                string cd = WorkingDirectory;
                if (cd != account.RemotePath)
                {
                    WorkingDirectory = account.RemotePath;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Throttle the file transfer if speed limits apply.
        /// </summary>
        /// <param name="limit">The download or upload rate to limit to, in kB/s.</param>
        /// <param name="transfered">bytes already transferred.</param>
        /// <param name="startedOn">when did the transfer start.</param>
        private void ThrottleTransfer(int limit, long transfered, DateTime startedOn)
        {
            var elapsed = DateTime.Now.Subtract(startedOn);
            var rate = (int)(elapsed.TotalSeconds < 1 ? transfered : transfered / elapsed.TotalSeconds);
            if (limit > 0 && rate > 1000 * limit)
            {
                double millisecDelay = (transfered / limit - elapsed.Milliseconds);

                if (millisecDelay > Int32.MaxValue)
                    millisecDelay = Int32.MaxValue;

                Thread.Sleep((int)millisecDelay);
            }
        }

        /// <summary>
        /// Returns the file size of the file in the given bath, in both SFTP and FTP
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>The file's size</returns>
        private long SizeOf(string path)
        {
            return _ftpc.GetFileSize(path);
        }

        /// <summary>
        /// Does the specified path exist on the remote folder?
        /// </summary>
        private bool Exists(string cpath)
        {
            return _ftpc.FileExists(cpath) || _ftpc.DirectoryExists(cpath);
        }

        /// <summary>
        /// Returns the LastWriteTime of the specified file/folder
        /// </summary>
        /// <param name="path">The common path to the file/folder</param>
        /// <returns></returns>
        private DateTime GetLwtOf(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return DateTime.MinValue;

            if (path.StartsWith("/")) path = path.Substring(1);
            var dt = DateTime.MinValue;

            try
            {
                dt = _ftpc.GetModifiedTime(path);
            }
            catch (Exception ex)
            {
            }

            return dt;
        }

        /// <summary>
        /// Convert FtpFileSystemObjectType to ClientItemType
        /// </summary>
        private ClientItemType _ItemTypeOf(FtpFileSystemObjectType f)
        {
            if (f == FtpFileSystemObjectType.File)
                return ClientItemType.File;
            if (f == FtpFileSystemObjectType.Directory)
                return ClientItemType.Folder;
            return ClientItemType.Other;
        }

        #endregion

        #region Properties

        private bool isConnected
        {
            get
            {
                return _ftpc.IsConnected;
            }
        }

        private bool ListingFailed { get; set; }

        private string WorkingDirectory
        {
            get
            {
                return _ftpc.GetWorkingDirectory();
            }
            set
            {
                _ftpc.SetWorkingDirectory(value);
            }
        }

        #endregion

        #region Listing

        /// <summary>
        /// Returns a non-recursive list of files/folders inside the specified path       
        /// </summary>
        /// <param name="cpath">path to folder to list inside</param>
        /// <param name="skipIgnored">if true, ignored items are not returned</param>
        private IEnumerable<ClientItem> List(string cpath, bool skipIgnored = true)
        {
            ListingFailed = false;
            UnsetKeepAlive();

            var list = new List<ClientItem>();

            try
            {
                list = Array.ConvertAll(new List<FtpListItem>(_ftpc.GetListing(cpath)).ToArray(), ConvertItem).ToList();
            }
            catch (Exception ex)
            {
                ListingFailed = true;
                yield break;
            }

            list.RemoveAll(x => x.Name == "." || x.Name == "..");
            if (skipIgnored)
                list.RemoveAll(x => x.FullPath.Contains("webint"));

            foreach (var f in list.Where(x => x.Type != ClientItemType.Other))
                yield return f;

            SetKeepAlive();
        }

        /// <summary>
        /// Get a full list of files/folders inside the specified path
        /// </summary>
        /// <param name="cpath">path to folder to list inside</param>
        /// <param name="skipIgnored">if true, ignored items are not returned</param>
        private IEnumerable<ClientItem> ListRecursive(string cpath, bool skipIgnored = true)
        {
            var list = new List<ClientItem>(List(cpath, skipIgnored).ToList());
            if (ListingFailed) yield break;

            if (skipIgnored)
                //list.RemoveAll(x => !controller.ItemGetsSynced(x.FullPath, false));

                foreach (var f in list.Where(x => x.Type == ClientItemType.File))
                    yield return f;

            foreach (var d in list.Where(x => x.Type == ClientItemType.Folder))
                foreach (var f in ListRecursiveInside(d, skipIgnored))
                    yield return f;
        }

        /// <summary>
        /// Returns a fully recursive listing inside the specified (directory) item
        /// </summary>
        /// <param name="p">The clientItem (should be of type directory) to list inside</param>
        /// <param name="skipIgnored">if true, ignored items are not returned</param>
        private IEnumerable<ClientItem> ListRecursiveInside(ClientItem p, bool skipIgnored = true)
        {
            yield return p;

            var cpath = GetCommonPath(p.FullPath);

            var list = new List<ClientItem>(List(cpath, skipIgnored).ToList());
            if (ListingFailed) yield break;

            if (skipIgnored)
                //list.RemoveAll(x => !controller.ItemGetsSynced(x.FullPath, false));

                foreach (var f in list.Where(x => x.Type == ClientItemType.File))
                    yield return f;

            foreach (var d in list.Where(x => x.Type == ClientItemType.Folder))
                foreach (var f in ListRecursiveInside(d, skipIgnored))
                    yield return f;
        }

        /// <summary>
        /// Convert an FtpItem to a ClientItem
        /// </summary>
        private ClientItem ConvertItem(FtpListItem f)
        {
            var fullPath = f.FullName;
            if (fullPath.StartsWith("./"))
            {
                var cwd = WorkingDirectory;
                var wd = (account.RemotePath != null && cwd.StartsWithButNotEqual(account.RemotePath) && cwd != "/") ? cwd : GetCommonPath(cwd);
                fullPath = fullPath.Substring(2);
                if (wd != "/")
                    fullPath = string.Format("/{0}/{1}", wd, fullPath);
                fullPath = fullPath.Replace("//", "/");
            }

            return new ClientItem
            {
                Name = f.Name,
                FullPath = fullPath,
                Type = _ItemTypeOf(f.Type),
                Size = f.Size,
                LastWriteTime = f.Modified
            };
        }

        /// <summary>
        /// Gets the common path of both local and remote directories.
        /// </summary>
        /// <returns>
        /// The common path, using forward slashes ( / )
        /// </returns>
        /// <param name='p'>
        /// The full path to be 'shortened'
        /// </param>
        private string GetCommonPath(string p)
        {
            // Remove the remote path from the begining
            if (account.RemotePath != null && p.StartsWith(account.RemotePath))
            {
                if (p.StartsWithButNotEqual(account.RemotePath))
                    p = p.Substring(account.RemotePath.Length);
            }
            // If path starts with homepath instead, remove the home path from the begining
            else if (account.HomePath != String.Empty && !account.HomePath.Equals("/"))
            {
                if (p.StartsWithButNotEqual(account.HomePath) || p.StartsWithButNotEqual(account.HomePath.RemoveSlashes()) || p.RemoveSlashes().StartsWithButNotEqual(account.HomePath))
                    p = p.Substring(account.HomePath.Length + 1);
                // ... and then remove the remote path
                if (account.RemotePath != null && p.StartsWithButNotEqual(account.RemotePath))
                    p = p.Substring(account.RemotePath.Length);
            }

            p = p.RemoveSlashes();
            if (p.StartsWithButNotEqual("/"))
                p = p.Substring(1);
            if (p.StartsWith("./"))
                p = p.Substring(2);

            if (String.IsNullOrWhiteSpace(p))
                p = "/";

            return p.ReplaceSlashes();
        }

        #endregion

    }

    public class Account
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public int SyncFrequency { get; set; }
        public string PrivateKeyFile { get; set; }
        public string HomePath { get; set; }
        public string RemotePath { get; set; }
        public long KeepAliveInterval = 10;
    }

    public class ClientItem
    {
        public ClientItem() { }

        public ClientItem(string name, string path, ClientItemType type, long size = 0x0, DateTime lastWriteTime = default(DateTime))
        {
            Name = name;
            FullPath = path;
            Type = type;
            Size = size;
            LastWriteTime = lastWriteTime;
        }

        #region Properties

        public string Name { get; set; }

        public string FullPath { get; set; }

        public string NewFullPath { get; set; }

        public ClientItemType Type { get; set; }

        public long Size { get; set; }

        public DateTime LastWriteTime { get; set; }

        #endregion
    }

    public enum ClientItemType
    {
        File,
        Folder,
        Other
    }

    public enum FtpFileType
    {
        Document,
        Picture,
        Thumbnail,
        ViolationPicture,
        ViolationThumbnail
    }
}
