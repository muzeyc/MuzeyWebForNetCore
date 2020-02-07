using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using MuzeyServer;
using System.Reflection;

namespace CommonUtils
{
    public static class ExcelUtil
    {        
        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="base64">图片base64码</param>
        /// <param name="tempPath">模板路径</param>
        /// <param name="outputPath">输出路径</param>
        /// <param name="col1">图片起始列</param>
        /// <param name="row1">图片起始行</param>
        /// <param name="col2">图片结束列</param>
        /// <param name="row2">图片结束行</param>
        /// <returns></returns>
        public static string AddImageToExcel(IWorkbook workbook, string base64, string tempPath, string outputPath, int col1, int row1, int col2, int row2)
        {
            byte[] bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
            int pictureIdx = workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
            ISheet sheet = workbook.GetSheetAt(0);
            HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();

            //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
            HSSFClientAnchor anchor = new HSSFClientAnchor(0, 0, 0, 0, col1, row1, col2, row2);
            HSSFPicture pict = (HSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);

            string outFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string resPath = outputPath + outFileName + ".xls";
            return resPath;
        }

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="bmp">图片</param>
        /// <param name="col1">图片起始列</param>
        /// <param name="row1">图片起始行</param>
        /// <param name="col2">图片结束列</param>
        /// <param name="row2">图片结束行</param>
        /// <returns></returns>
        public static void AddImageToExcel(IWorkbook workbook, int sheetIndex, Bitmap bmp, int col1, int row1, int col2, int row2)
        {
            string base64 = ImgToBase64String(bmp);
            byte[] bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
            int pictureIdx = workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            XSSFDrawing patriarch = (XSSFDrawing)sheet.CreateDrawingPatriarch();

            //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
            XSSFClientAnchor anchor = new XSSFClientAnchor(100*10000, 0, 100, 100, col1, row1, col2, row2);
            patriarch.CreatePicture(anchor, pictureIdx);
        }

        /// <summary>
        /// 将Base64字符串转换为图片
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="bmp">图片</param>
        /// <param name="col1">图片起始列</param>
        /// <param name="row1">图片起始行</param>
        /// <param name="col2">图片结束列</param>
        /// <param name="row2">图片结束行</param>
        /// <returns></returns>
        public static void AddImageToExcel(ISheet sheet, Bitmap bmp, int col1, int row1, int col2, int row2)
        {
            string base64 = ImgToBase64String(bmp);
            byte[] bytes = Convert.FromBase64String(base64.Replace(" ", "+"));
            int pictureIdx = sheet.Workbook.AddPicture(bytes, NPOI.SS.UserModel.PictureType.JPEG);
            XSSFDrawing patriarch = (XSSFDrawing)sheet.CreateDrawingPatriarch();

            //##处理照片位置，【图片左上角为（6, 2）第2+1行6+1列，右下角为（8, 6）第6+1行8+1列】
            XSSFClientAnchor anchor = new XSSFClientAnchor(100 * 10000, 0, 100, 100, col1, row1, col2, row2);
            patriarch.CreatePicture(anchor, pictureIdx);
        }

        //图片转为base64编码的字符串  
        private static string ImgToBase64String(Bitmap bmp)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch (Exception ex)
            {
                return null;
            }
        }  

        /// <summary>
        /// 将DataTable数组导入Excel
        /// </summary>
        /// <param name="dts"></param>
        /// <returns></returns>
        public static IWorkbook DataTableToExcel(DataTable[] dts)
        {
            ISheet sheet = null;
            IWorkbook workbook = new XSSFWorkbook();
            try
            {
                int rowNum = 0;
                foreach (DataTable dt in dts)
                {
                    sheet = workbook.CreateSheet(dt.TableName);
                    IRow row = sheet.CreateRow(rowNum);

                    for (int j = 0; j < dt.Columns.Count; ++j)
                    {
                        row.CreateCell(j).SetCellValue(dt.Columns[j].ColumnName);
                        sheet.AutoSizeColumn(j);
                    }

                    rowNum++;

                    for (int i = 0; i < dt.Rows.Count; ++i)
                    {
                        row = sheet.CreateRow(rowNum);
                        for (int j = 0; j < dt.Columns.Count; ++j)
                        {
                            row.CreateCell(j).SetCellValue(dt.Rows[i][j].ToString());
                            sheet.AutoSizeColumn(j);
                        }
                        rowNum++;
                    }
                }

                return workbook;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 将excel中的数据导入到DataTable中
        /// </summary>
        /// <param name="filePath">excel文件路径</param>
        /// <param name="sheetName">excel工作薄sheet的名称</param>
        /// <param name="isFirstRowColumn">第一行是否是DataTable的列名</param>
        /// <returns>返回的DataTable</returns>
        public static DataTable ExcelToDataTable(string filePath, string sheetName, bool isFirstRowColumn)
        {
            ISheet sheet = null;
            DataTable data = new DataTable();
            int startRow = 0;
            try
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                IWorkbook workbook = null;
                if (filePath.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (filePath.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);

                if (sheetName != null)
                {
                    sheet = workbook.GetSheet(sheetName);
                    if (sheet == null) //如果没有找到指定的sheetName对应的sheet，则尝试获取第一个sheet
                    {
                        sheet = workbook.GetSheetAt(0);
                    }
                }
                else
                {
                    sheet = workbook.GetSheetAt(0);
                }
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum; //一行最后一个cell的编号 即总的列数

                    if (isFirstRowColumn)
                    {
                        int columnNum = 0;
                        for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                        {
                            ICell cell = firstRow.GetCell(i);
                            if (cell != null)
                            {
                                string cellValue = cell.StringCellValue;
                                if (cellValue != null)
                                {
                                    DataColumn column = new DataColumn(cellValue + "#" + columnNum);
                                    data.Columns.Add(column);
                                    columnNum++;
                                }
                            }
                        }
                        startRow = sheet.FirstRowNum + 1;
                    }
                    else
                    {
                        startRow = sheet.FirstRowNum;
                    }

                    //最后一列的标号
                    int rowCount = sheet.LastRowNum;
                    for (int i = startRow; i <= rowCount; ++i)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null) continue; //没有数据的行默认是null　　　　　　　

                        DataRow dataRow = data.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        data.Rows.Add(dataRow);
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 将list数组导入Excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sheetName"></param>
        /// <param name="useDesc">是否使用注释做列头</param>
        /// <returns></returns>
        public static IWorkbook ListToExcel<T>(List<T> list, string sheetName, bool useDesc = true)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            if (list != null && list.Count > 0)
            {
                var mo = list[0];
                //获得该类的Type
                Type t = mo.GetType();
                var props = t.GetProperties();
                int rowNum = 0;

                IRow row = sheet.CreateRow(rowNum);
                for (int i = 0; i < props.Length; i++)
                {
                    string colName = "";
                    if (useDesc)
                    {
                        object[] objs = props[i].GetCustomAttributes(typeof(DescriptionAttribute), true);
                        colName = ((DescriptionAttribute)objs[0]).Description;
                    }
                    else
                    {
                        colName = props[i].Name;
                    }
                    row.CreateCell(i).SetCellValue(colName);
                    sheet.AutoSizeColumn(i);
                }
                rowNum++;

                foreach (T model in list)
                {
                    row = sheet.CreateRow(rowNum);
                    for (int i = 0; i < props.Length; i++)
                    {
                        row.CreateCell(i).SetCellValue(props[i].GetValue(model).ToStr());
                        //sheet.AutoSizeColumn(i);
                    }
                    rowNum++;
                }
            }

            return workbook;
        }

        /// <summary>
        /// 将list数组导入Excel
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sheetName"></param>
        /// <param name="useDesc">是否使用注释做列头</param>
        /// <returns></returns>
        public static IWorkbook ListToExcel<T>(List<T> list, string sheetName, List<MuzeyColModel> mcms)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            if (list != null && list.Count > 0)
            {
                var mo = list[0];
                //获得该类的Type
                Type t = mo.GetType();
                var props = t.GetProperties();
                var propsDic = new Dictionary<string, PropertyInfo>();
                foreach(var p in props)
                {
                    propsDic.Add(p.Name, p);
                }
                int rowNum = 0;

                IRow row = sheet.CreateRow(rowNum);
                for (int i = 0; i < mcms.Count; i++)
                {
                    string colName = "";
                    colName = mcms[i].label;
                    row.CreateCell(i).SetCellValue(colName);
                    sheet.AutoSizeColumn(i);
                }
                rowNum++;

                foreach (T model in list)
                {
                    row = sheet.CreateRow(rowNum);
                    for (int i = 0; i < mcms.Count; i++)
                    {
                        row.CreateCell(i).SetCellValue(propsDic[mcms[i].name].GetValue(model).ToStr());
                        //sheet.AutoSizeColumn(i);
                    }
                    rowNum++;
                }
            }

            return workbook;
        }

        /// <summary>
        /// 保存Excel
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="fileName"></param>
        public static void CreateExcel(IWorkbook workbook, string fileName)
        {
            FileStream fs = File.Create(fileName);
            workbook.Write(fs);
            fs.Close();
        }

        /// <summary>
        /// 判断单元格数据类型并返回字符型数据
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static string CellValueToString(this ICell cell)
        {
            if (cell == null)
            {
                return string.Empty;
            }
            if (cell.CellType == CellType.String)
            {
                return CommonUtil.ToStr(cell.StringCellValue);
            }
            else if (cell.CellType == CellType.Numeric)
            {
                return CommonUtil.ToStr(cell.NumericCellValue);
            }
            else if (cell.CellType == CellType.Formula)
            {
                if (cell.CachedFormulaResultType == CellType.String)
                {
                    return CommonUtil.ToStr(cell.StringCellValue);
                }
                else if (cell.CachedFormulaResultType == CellType.Numeric)
                {
                    return CommonUtil.ToStr(cell.NumericCellValue);
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 复制并插入行
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sourceRowNo"></param>
        /// <param name="targetRow"></param>
        /// <param name="count"></param>
        public static void CopyAndInsertRow(ISheet sheet, int sourceRowNo, int targetRowNo, int rowCount)
        {
            if (rowCount <= 0)
            {
                return;
            }

            IRow sourceRow = sheet.GetRow(sourceRowNo);
            var dicSpan = new Dictionary<int, int>();
            int rowspan = 0;
            int colspan = 0;
            for (int m = sourceRow.FirstCellNum; m < sourceRow.LastCellNum;)
            {
                bool isMerge = IsMergeCell(sheet, sourceRowNo + 1, m + 1, out rowspan, out colspan);
                if (isMerge)
                {
                    dicSpan.Add(m, colspan);
                    m += colspan;
                }
                else
                {
                    m++;
                }
            }

            //Dictionary<int, short> dic = new Dictionary<int, short>();

            //for (int i = targetRowNo; i <= sheet.LastRowNum; i++)
            //{
            //    if (sheet.GetRow(i) == null)
            //    {
            //        continue;
            //    }
            //    dic.Add(i + rowCount, sheet.GetRow(i).Height);
            //}

            sheet.ShiftRows(targetRowNo, sheet.LastRowNum, rowCount, true, true);

            ICell sourceCell = null;
            ICell targetCell = null;
            IRow targetRow = null;

            for (int i = 0; i < rowCount; i++)
            {
                targetRow = sheet.CreateRow(targetRowNo + i);
                for (int m = sourceRow.FirstCellNum; m < sourceRow.LastCellNum; m++)
                {
                    sourceCell = sourceRow.GetCell(m);
                    if (sourceCell == null)
                    {
                        continue;
                    }
                    targetCell = targetRow.CreateCell(m);
                    targetCell.CellStyle = sourceCell.CellStyle;
                    targetCell.SetCellType(sourceCell.CellType);
                    targetCell.SetCellValue(CellValueToString(sourceCell));
                    if (dicSpan.ContainsKey(m))
                    {
                        sheet.AddMergedRegion(new CellRangeAddress(targetRow.RowNum, targetRow.RowNum, m, m + dicSpan[m] - 1));
                    }
                }
                //sheet.CopyRow(sourceRowNo, targetRowNo + i);
                targetRow.Height = sourceRow.Height;

                if (sourceRowNo >= targetRowNo)
                {
                    sourceRowNo++;
                }
            }

            //foreach (var item in dic)
            //{
            //    sheet.GetRow(item.Key).Height = dic[item.Key];
            //}
        }

        /// <summary>
        /// 判断合并单元格重载
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowNum"></param>
        /// <param name="colNum"></param>
        /// <param name="rowSpan"></param>
        /// <param name="colSpan"></param>
        /// <returns></returns>
        public static bool IsMergeCell(ISheet sheet, int rowNum, int colNum, out int rowSpan, out int colSpan)
        {
            bool result = false;
            rowSpan = 0;
            colSpan = 0;
            if ((rowNum < 1) || (colNum < 1)) return result;
            int rowIndex = rowNum - 1;
            int colIndex = colNum - 1;
            int regionsCount = sheet.NumMergedRegions;
            rowSpan = 1;
            colSpan = 1;
            for (int i = 0; i < regionsCount; i++)
            {
                CellRangeAddress range = sheet.GetMergedRegion(i);
                sheet.IsMergedRegion(range);
                if (range.FirstRow == rowIndex && range.FirstColumn == colIndex)
                {
                    rowSpan = range.LastRow - range.FirstRow + 1;
                    colSpan = range.LastColumn - range.FirstColumn + 1;
                    break;
                }
            }
            try
            {
                result = sheet.GetRow(rowIndex).GetCell(colIndex).IsMergedCell;
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 给每个单元格设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="row"></param>
        public static void SetRowDate<T>(T obj, IRow row)
        {
            if (row == null) return;
            var dic = new Dictionary<string, object>();
            var type = obj.GetType();
            var ps = type.GetProperties();
            foreach (var p in ps)
            {
                dic[p.Name] = p.GetValue(obj);
            }
            var last = row.LastCellNum;
            for (int i = 0; i < last; i++)
            {
                string txt = CellValueToString(row.GetCell(i)).Trim();
                if (!txt.IsNullOrEmpty())
                {
                    if (dic.ContainsKey(txt))
                    {
                        row.GetCell(i).SetCellValue(dic[txt].ToStr());
                    }
                }
            }
        }

        /// <summary>
        /// 给每个单元格设置值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="sheet"></param>
        public static void SetSheetDate<T>(T obj, ISheet sheet)
        {
            var dic = new Dictionary<string, object>();
            var type = obj.GetType();
            var ps = type.GetProperties();
            foreach (var p in ps)
            {
                dic[p.Name] = p.GetValue(obj);
            }

            var lastRow = sheet.LastRowNum;
            for (int i = 0; i <= lastRow; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var lastCell = row.LastCellNum;
                for (int j = 0; j < lastCell; j++)
                {
                    string txt = CellValueToString(row.GetCell(j));
                    if (!txt.IsNullOrEmpty())
                    {
                        if (dic.ContainsKey(txt))
                        {
                            row.GetCell(j).SetCellValue(dic[txt].ToStr());
                        }
                    }
                }
            }
        }

        public static byte[] GetExcelBs(IWorkbook wb, string fn)
        {
            var path = "DownFiles/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var fileName = path + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fn;
            var fs = new FileStream(fileName, FileMode.OpenOrCreate);
            wb.Write(fs);
            fs = new FileStream(fileName, FileMode.OpenOrCreate);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();

            return bytes;
        }
    }
}
