//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Nick Ma 
//-----------------------------------------------------------------

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DotNet.Business;
using DotNet.Utilities;
using System.Windows.Forms;
using System.Collections.Generic;

namespace WafTraffic.Applications.Utils
{
    /// <summary>
    /// <author>
    ///		<name>Nick</name>
    ///		<date>2015.04.01</date>
    /// </author> 
    /// </summary>
    public class VersionUpdateUtil
    {
        public static ClientItem updateItem = null;

        public static string GetVersionNumber(string versionString)
        {
            string versionNumber = string.Empty;
            string tmpString = string.Empty;

            if (versionString != null && versionString != string.Empty)
            {
                // 正则表达式剔除后缀名
                tmpString = Regex.Replace(versionString, ".msi", "", RegexOptions.IgnoreCase);

                // 正则表达式剔除非数字字符（不包含小数点.） 
                versionNumber = Regex.Replace(tmpString, @"[^\d.\d]", "");
                // 如果是数字，则转换为decimal类型 
            }

            return versionNumber;
        }

        public static bool NeedUpdate()
        {
            bool result = false;
            string localVersion;
            string remoteVersion;

            FtpHelper ftp = null;

            try
            {
                ftp = new FtpHelper();
                List<ClientItem> fileList = ftp.GetAllFiles(ftp.VersionsRoot);

                localVersion = GetVersionNumber(BaseSystemInfo.Version);

                foreach (ClientItem item in fileList)
                {
                    remoteVersion = GetVersionNumber(item.Name);

                    if (localVersion != null && remoteVersion != string.Empty)
                    {
                        if (Convert.ToDouble(localVersion) < Convert.ToDouble(remoteVersion))
                        {
                            updateItem = item;
                            result = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }

            return result;
        }


        public static bool DownloadSoftware()
        {
            bool ret = true;
            string fileFrom = string.Empty;
            string fileTo = string.Empty;

            FtpHelper ftp = null;

            try
            {
                SaveFileDialog sf = new SaveFileDialog();

                sf.AddExtension = true;
                sf.RestoreDirectory = true;
                sf.FileName = updateItem.Name;
                if (sf.ShowDialog() == DialogResult.OK)
                {
                    fileFrom = updateItem.FullPath;
                    fileTo = sf.FileName;

                    if (ValidateUtil.IsBlank(fileFrom) || ValidateUtil.IsBlank(fileTo))
                    {
                        MessageBox.Show("文件路径不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ret = false;
                        return ret;
                    }

                    ftp = new FtpHelper();

                    ftp.DownloadFile(fileFrom, fileTo);
                }
                else
                {
                    ret = false;
                }

            }
            catch (System.Exception ex)
            {
                ret = false;
                MessageBox.Show("下载失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                CurrentLoginService.Instance.LogException(ex);
            }
            finally
            {
                if (ftp != null)
                {
                    ftp.Disconnect();
                    ftp = null;
                }
            }

            return ret;
        }
    }
}