using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace automated_electrical_schedule.Extensions;

public static class ExcelWorksheetExtensions
{
    private static readonly List<string> excelColumns = Enumerable.Range('A', 26).Select(c => ((char)c).ToString()).ToList();

    public static ExcelRange GetRange(
        this ExcelWorksheet excelWorksheet, 
        int startColIndex, 
        int startRow,
        int? endColIndex = null,
        int? endRow = null
    )
    {
        endColIndex ??= startColIndex;
        endRow ??= startRow;
        
        return excelWorksheet.Cells[$"{excelColumns[startColIndex]}{startRow}:{excelColumns[endColIndex.Value]}{endRow.Value}"];
    }
    
    public static void InitSchedCell(
        this ExcelWorksheet excelWorksheet,
        string value,
        int colIndex,
        int row
    )
    {
        var cell = excelWorksheet.Cells[$"{excelColumns[colIndex]}{row}"];
        cell.Value = value;
        cell.Style.Font.Bold = true;
        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
    }
    
    public static void InitSchedRange(
        this ExcelWorksheet excelWorksheet,
        string value,
        int startColIndex,
        int startRow,
        int? endColIndex = null,
        int? endRow = null,
        bool shouldBorder = true
    )
    {
        if (endColIndex is null && endRow is null)
        {
            InitSchedCell(
                excelWorksheet,
                value,
                startColIndex,
                startRow
            );

            return;
        }
        
        var range = GetRange(
            excelWorksheet,
            startColIndex,
            startRow,
            endColIndex,
            endRow
        );

        range.Merge = true;
        if (shouldBorder) range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
        range.Value = value;
    }

    public static void InitCompCell(
        this ExcelWorksheet excelWorksheet,
        string value,
        int colIndex,
        int row,
        int? fontSize = null,
        bool bold = false,
        bool underline = false
    )
    {
        var cell = excelWorksheet.Cells[$"{excelColumns[colIndex]}{row}"];
        cell.Value = value;

        if (fontSize is not null) cell.Style.Font.Size = fontSize.Value;
        if (bold) cell.Style.Font.Bold = true;
        if (underline) cell.Style.Font.UnderLine = true;
    }
}