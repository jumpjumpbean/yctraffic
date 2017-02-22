using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DotNet.Business;
using DotNet.Utilities;
using WafTraffic.Applications.Common;
using System.Windows.Forms;
using WafTraffic.Domain;
using Excel;
using System.Data;
using System.ComponentModel.Composition;

namespace WafTraffic.Applications.Utils
{
    [Export]
    public class ExcelReader
    {
        #region Data

        private static ExcelReader instance = null;
        private static object locker = new object();

        #endregion

        #region Constructor

        public static ExcelReader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ExcelReader();
                            //instance.Initialize();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        #region Member

        public IEnumerable<ZhzxTrafficViolationExt> ReadTrafficViolationExcel(string filePath)
        {
            List<ZhzxTrafficViolationExt> entities = null;
            try
            {
                if (ValidateUtil.IsBlank(filePath))
                {
                    return null;
                }
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("Excel文件不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                if(IsFileInUse(filePath))
                {
                    MessageBox.Show("Excel已被打开，请关闭后重试！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                int dotIndex = filePath.LastIndexOf(".");
                if(-1 == dotIndex)
                {
                    MessageBox.Show("Excel文件名不正确！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string dirctoryName = Path.GetDirectoryName(filePath);
                IExcelDataReader excelReader = null;
                if (filePath.Substring(dotIndex+1).Equals("xls"))
                {
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (filePath.Substring(dotIndex+1).Equals("xlsx"))
                {
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    MessageBox.Show("Excel文件类型不正确，请选择xls或xlsx文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                
                DataSet dtExcel = excelReader.AsDataSet();
                excelReader.Close();
                DataTable table = dtExcel.Tables[0];
                if (TrafficViolationExcelFormatCheck(table.Rows[YcConstants.INT_ZHZX_ROW_TITLE]))
                {
                    int row = YcConstants.INT_ZHZX_ROW_TITLE + 1;
                    int j, blankCnt;
                    entities = new List<ZhzxTrafficViolationExt>();
                    for (; row < table.Rows.Count; row++)
                    {
                        blankCnt = 0;

                        //判断前六列为空，即为空行，跳过
                        for (j = 0; j < 6; j++)
                        {
                            if (string.IsNullOrEmpty(table.Rows[row][j].ToString().Trim()))
                            {
                                blankCnt++;
                            }
                        }

                        if (blankCnt >= 4)
                        {
                            continue;
                        }

                        ZhzxTrafficViolation entity = new ZhzxTrafficViolation();
                        ZhzxTrafficViolationExt extEntity = new ZhzxTrafficViolationExt();
                        entity.CheckpointName = table.Rows[row][YcConstants.INT_ZHZX_COL_CHECK_POINT_NAME].ToString();
                        entity.CaptureLocation = table.Rows[row][YcConstants.INT_ZHZX_COL_CAPTURE_LOCATION].ToString();
                        entity.LicensePlateNumber = table.Rows[row][YcConstants.INT_ZHZX_COL_LICENSE_PLATE_NUMBER].ToString();
                        entity.OwnershipOfLand = table.Rows[row][YcConstants.INT_ZHZX_COL_OWNERSHIP_OF_LAND].ToString();
                        entity.Speed = table.Rows[row][YcConstants.INT_ZHZX_COL_SPEED].ToString();
                        entity.ViolationType = table.Rows[row][YcConstants.INT_ZHZX_COL_VIOLATION_TYPE].ToString();
                        entity.VehicleType = table.Rows[row][YcConstants.INT_ZHZX_COL_VEHICLE_TYPE].ToString();
                        entity.LicensePlateColor = table.Rows[row][YcConstants.INT_ZHZX_COL_LICENSE_PLATE_COLOR].ToString();
                        entity.VehicleColor = table.Rows[row][YcConstants.INT_ZHZX_COL_VEHICLE_COLOR].ToString();

                        if (!string.IsNullOrEmpty(table.Rows[row][YcConstants.INT_ZHZX_COL_CAPTURE_TIME].ToString()))
                        {
                            entity.CaptureTime = DateTime.Parse(table.Rows[row][YcConstants.INT_ZHZX_COL_CAPTURE_TIME].ToString());
                        }

                        entity.DataStatus = table.Rows[row][YcConstants.INT_ZHZX_COL_DATA_STATUS].ToString();
                        entity.ExcelName = fileName;
                        extEntity.ZhzxTrafficViolationEntity = entity;
                        extEntity.ComposedPicturePath = CombinePicturePath(dirctoryName, table.Rows[row][YcConstants.INT_ZHZX_COL_COMPOSED_PICTURE].ToString(), fileName);
                        //extEntity.SourcePicturePath1 = CombinePicturePath(dirctoryName, table.Rows[row][YcConstants.INT_ZHZX_COL_SOURCE_PICTURE_1].ToString(), fileName);
                        //extEntity.SourcePicturePath2 = CombinePicturePath(dirctoryName, table.Rows[row][YcConstants.INT_ZHZX_COL_SOURCE_PICTURE_2].ToString(), fileName);
                        //extEntity.SourcePicturePath3 = CombinePicturePath(dirctoryName, table.Rows[row][YcConstants.INT_ZHZX_COL_SOURCE_PICTURE_3].ToString(), fileName);

                        entities.Add(extEntity);
                    }
                }
                else
                {
                    MessageBox.Show("Excel格式不正确，无法识别", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            catch (System.Exception ex)
            {
                CurrentLoginService.Instance.LogException(ex);
            }
            return entities;
        }

        private string CombinePicturePath(string dir, string src, string cur)
        {
            string combinedPath = string.Empty;

            if (ValidateUtil.IsBlank(dir) || ValidateUtil.IsBlank(src) || ValidateUtil.IsBlank(cur))
            {
                return string.Empty;
            }

            if (!src.Contains(cur)) return string.Empty;

            combinedPath = dir + "\\" + src.Substring(src.IndexOf(cur));

            return combinedPath;
        }

        private bool TrafficViolationExcelFormatCheck(DataRow row)
        {
            bool result = false;

            if (row == null) return result;
            if (row[YcConstants.INT_ZHZX_COL_CHECK_POINT_NAME].ToString().Equals(YcConstants.STR_ZHZX_COL_CHECK_POINT_NAME)
                && row[YcConstants.INT_ZHZX_COL_CAPTURE_LOCATION].ToString().Equals(YcConstants.STR_ZHZX_COL_CAPTURE_LOCATION)
                && row[YcConstants.INT_ZHZX_COL_LICENSE_PLATE_NUMBER].ToString().Equals(YcConstants.STR_ZHZX_COL_LICENSE_PLATE_NUMBER)
                && row[YcConstants.INT_ZHZX_COL_OWNERSHIP_OF_LAND].ToString().Equals(YcConstants.STR_ZHZX_COL_OWNERSHIP_OF_LAND)
                && row[YcConstants.INT_ZHZX_COL_SPEED].ToString().Equals(YcConstants.STR_ZHZX_COL_SPEED)
                && row[YcConstants.INT_ZHZX_COL_VIOLATION_TYPE].ToString().Equals(YcConstants.STR_ZHZX_COL_VIOLATION_TYPE)
                && row[YcConstants.INT_ZHZX_COL_LICENSE_PLATE_COLOR].ToString().Equals(YcConstants.STR_ZHZX_COL_LICENSE_PLATE_COLOR)
                && row[YcConstants.INT_ZHZX_COL_VEHICLE_COLOR].ToString().Equals(YcConstants.STR_ZHZX_COL_VEHICLE_COLOR)
                && row[YcConstants.INT_ZHZX_COL_CAPTURE_TIME].ToString().Equals(YcConstants.STR_ZHZX_COL_CAPTURE_TIME)
                && row[YcConstants.INT_ZHZX_COL_DATA_STATUS].ToString().Equals(YcConstants.STR_ZHZX_COL_DATA_STATUS)
                )
            {
                result = true;
            }

            return result;
        }

        public bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            if (File.Exists(fileName))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    inUse = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
                return inUse;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
