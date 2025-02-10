using automated_electrical_schedule.Data.Enums;
using automated_electrical_schedule.Data.FormulaTables;
using automated_electrical_schedule.Data.Models;
using automated_electrical_schedule.Extensions;
using CommunityToolkit.Maui.Storage;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace automated_electrical_schedule.Data;

public class ExcelService
{
    private string ProjectName { get; }
    
    private List<DistributionBoard> DistributionBoards { get; }
    
    public ExcelService(string projectName, List<DistributionBoard> distributionBoards)
    {
        ProjectName = projectName;
        DistributionBoards = distributionBoards;
    }
    
    public async Task Export()
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
        using var package = new ExcelPackage();
        
        foreach (var board in DistributionBoards)
        {
            _createScheduleSheet(package, board);
            _createComputationsSheet(package, board);
        }
        
        using var stream = new MemoryStream(package.GetAsByteArray());
        await FileSaver.SaveAsync("Schedule of Loads.xlsx", stream);
    }

    private void _createScheduleSheet(ExcelPackage? package, DistributionBoard board)
    {
        var scheduleSheet = package!.Workbook.Worksheets.Add($"{board.BoardName}: SOL");
        scheduleSheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        scheduleSheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
        
        var threePhaseBoard = board as ThreePhaseDistributionBoard;
            
        // PROJECT & BOARD DESCRIPTION -------------------------------------
        scheduleSheet.Cells["A1:E1"].Merge = true;
        scheduleSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        scheduleSheet.Cells["A1"].Value = $"PROJECT NAME: {ProjectName}";
        
        scheduleSheet.Cells["F1:J1"].Merge = true;
        scheduleSheet.Cells["F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        var configText = threePhaseBoard is null
            ? ""
            : " " + threePhaseBoard.ThreePhaseConfiguration.GetDisplayName();
        scheduleSheet.Cells["F1"].Value =
            $"SYSTEM: {board.Phase.GetDisplayName()}{configText}";
            
        scheduleSheet.Cells["A2:E2"].Merge = true;
        scheduleSheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        scheduleSheet.Cells["A2"].Value = $"DISTRIBUTION BOARD NAME: {board.BoardName}";
            
        scheduleSheet.Cells["F2:J2"].Merge = true;
        scheduleSheet.Cells["F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        scheduleSheet.Cells["F2"].Value = $"VOLTAGE: {(int) board.Voltage} V";
        
        // TABLE HEADERS ---------------------------------------------------
        int colI = 0;
        int row = 3;

        scheduleSheet.InitRange("CKT NO", colI, row, colI, row + 1);
        colI += 1;
        
        scheduleSheet.InitRange("Load Description", colI, row, colI, row + 1);
        colI += 1;
        
        scheduleSheet.InitRange("QTY", colI, row, colI, row + 1);
        colI += 1;
        
        scheduleSheet.InitRange("VA", colI, row, colI, row + 1);
        colI += 1;
        
        scheduleSheet.InitRange("Voltage", colI, row, colI, row + 1);
        colI += 1;

        if (threePhaseBoard is null)
        {
            scheduleSheet.InitRange("Ampere Load", colI, row, colI, row + 1);
        }
        else
        {
            scheduleSheet.InitRange("Ampere Load", colI, row, colI + 3, row);
            row = 4;
            
            var headerA = threePhaseBoard.ThreePhaseConfiguration == ThreePhaseConfiguration.Delta ? "AB" : "AN";
            scheduleSheet.InitRange(headerA, colI, row);
            colI += 1;
            
            var headerB = threePhaseBoard.ThreePhaseConfiguration == ThreePhaseConfiguration.Delta ? "BC" : "BN";
            scheduleSheet.InitRange(headerB, colI, row);
            colI += 1;
            
            var headerC = threePhaseBoard.ThreePhaseConfiguration == ThreePhaseConfiguration.Delta ? "CA" : "CN";
            scheduleSheet.InitRange(headerC, colI, row);
            colI += 1;
            
            scheduleSheet.InitRange("ABC", colI, row);
            row = 3;
        }

        colI += 1;
        
        scheduleSheet.InitRange("Circuit Protection", colI, row, colI + 6, row);
        row = 4;
        
        scheduleSheet.InitRange("AT", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("AF", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Phase", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Pole", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Length", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("VD", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Type", colI, row);
        colI += 1;
        row = 3;
        
        scheduleSheet.InitRange("Conductor Size", colI, row, colI + 3, row);
        row = 4;
        
        scheduleSheet.InitRange("Sets", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Line+Neutral", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Ground", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Raceway", colI, row);
        
        // TABLE ROWS
        
        row = 5;

        foreach (var item in board.CircuitsAndSubBoards)
        {
            colI = 0;
            
            var circuit = item as Circuit;
            var fixtureCircuit = item as FixtureCircuit;
            var motorCircuit = item as MotorOutletCircuit;
            var convenienceCircuit = item as ConvenienceOutletCircuit;
            var nonSpaceCircuit = item as NonSpaceCircuit;
            var spaceCircuit = item as SpaceCircuit;
            var spareCircuit = item as SpareCircuit;
            var nonSpareCircuit = item as NonSpareCircuit;
            var subBoard = item as DistributionBoard;
        
            int rowSpan = 1;
            if (fixtureCircuit is not null && fixtureCircuit.IsItemized)
            {
                rowSpan = fixtureCircuit.Fixtures.Count + 1;
            } 
            else if (convenienceCircuit is not null)
            {
                if (convenienceCircuit.GfciReceptacleQuantity > 0) rowSpan += 1;
                if (convenienceCircuit.OneGangQuantity > 0) rowSpan += 1;
                if (convenienceCircuit.TwoGangQuantity > 0) rowSpan += 1;
                if (convenienceCircuit.ThreeGangQuantity > 0) rowSpan += 1;
                if (convenienceCircuit.FourGangQuantity > 0) rowSpan += 1;
            }
            
            scheduleSheet.InitRange(item.Order.ToString(), colI, row, colI, row + rowSpan - 1);
            colI += 1;

            if (spaceCircuit is not null)
            {
                scheduleSheet.InitRange("Space", colI, row);
                colI += 1;

                var remainingSpaceCols = threePhaseBoard is null ? 16 : 19;
                for (var i = 0; i < remainingSpaceCols; i++)
                {
                    scheduleSheet.InitRange(string.Empty, colI, row);
                    colI += 1;
                }
            } 
            else if (nonSpaceCircuit is not null)
            {
                var description = nonSpaceCircuit is IDescribed describedCircuit
                    ? describedCircuit.Description
                    : "Spare";
                var isDescriptionBordered =
                    (fixtureCircuit is not null && fixtureCircuit.IsItemized) || 
                    convenienceCircuit is not null;
                scheduleSheet.InitRange(description, colI, row, null, null, !isDescriptionBordered);
                colI += 1;

                int? quantity = null;
                if (fixtureCircuit is not null && !fixtureCircuit.IsItemized && fixtureCircuit.Fixtures.Count > 0)
                {
                    quantity = fixtureCircuit.Fixtures[0].Quantity;
                } 
                else if (motorCircuit is not null)
                {
                    quantity = 1;
                }
                scheduleSheet.InitRange(quantity?.ToString() ?? string.Empty, colI, row);
                colI += 1;

                var voltAmpereDisplay = string.Empty;
                if (spareCircuit is not null)
                {
                    voltAmpereDisplay = spareCircuit.VoltAmpere.ToRoundedString();
                } 
                else if (nonSpareCircuit is not null && !nonSpareCircuit.VoltAmpere.HasError)
                {
                    voltAmpereDisplay = nonSpareCircuit.VoltAmpere.Value.ToRoundedString();
                }
                scheduleSheet.InitRange(voltAmpereDisplay, colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(nonSpaceCircuit.Voltage.ToString(), colI, row);
                colI += 1;

                var circuitAmpereLoadDisplay = nonSpaceCircuit.AmpereLoad.HasError
                    ? string.Empty
                    : nonSpaceCircuit.AmpereLoad.Value.ToRoundedString();

                if (threePhaseBoard is null)
                {
                    scheduleSheet.InitRange(circuitAmpereLoadDisplay, colI, row);
                }
                else
                {
                    var circuitAmpereLoadADisplay = nonSpaceCircuit.LineToLineVoltage == LineToLineVoltage.A
                        ? circuitAmpereLoadDisplay
                        : string.Empty;
                    scheduleSheet.InitRange(circuitAmpereLoadADisplay, colI, row);
                    colI += 1;
                    
                    var circuitAmpereLoadBDisplay = nonSpaceCircuit.LineToLineVoltage == LineToLineVoltage.B
                        ? circuitAmpereLoadDisplay
                        : string.Empty;
                    scheduleSheet.InitRange(circuitAmpereLoadBDisplay, colI, row);
                    colI += 1;
                    
                    var circuitAmpereLoadCDisplay = nonSpaceCircuit.LineToLineVoltage == LineToLineVoltage.C
                        ? circuitAmpereLoadDisplay
                        : string.Empty;
                    scheduleSheet.InitRange(circuitAmpereLoadCDisplay, colI, row);
                    colI += 1;
                    
                    var circuitAmpereLoadAbcDisplay = nonSpaceCircuit.LineToLineVoltage == LineToLineVoltage.Abc
                        ? circuitAmpereLoadDisplay
                        : string.Empty;
                    scheduleSheet.InitRange(circuitAmpereLoadAbcDisplay, colI, row);
                }

                colI += 1;

                var ampereTripDisplay = nonSpaceCircuit.AmpereTrip.HasError
                    ? string.Empty
                    : nonSpaceCircuit.AmpereTrip.Value.ToString();
                scheduleSheet.InitRange(ampereTripDisplay, colI, row);
                colI += 1;

                var ampereFrameDisplay = nonSpaceCircuit.AmpereFrame.HasError
                    ? string.Empty
                    : nonSpaceCircuit.AmpereFrame.Value.ToString();
                scheduleSheet.InitRange(ampereFrameDisplay, colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(nonSpaceCircuit.Phase.ToString(), colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(nonSpaceCircuit.Pole.ToString(), colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(nonSpaceCircuit.WireLength.ToRoundedString(), colI, row);
                colI += 1;
                
                var voltageDropDisplay = nonSpaceCircuit.VoltageDrop.HasError
                    ? string.Empty
                    : nonSpaceCircuit.VoltageDrop.Value.ToRoundedString();
                scheduleSheet.InitRange(voltageDropDisplay, colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(nonSpaceCircuit.CircuitProtection.GetDisplayName(), colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(nonSpaceCircuit.SetCount.ToString(), colI, row);
                colI += 1;
                
                var conductorDisplay = nonSpaceCircuit.ConductorSize.HasError 
                    ? string.Empty
                    : $"{nonSpaceCircuit.ConductorWireCount}-{nonSpaceCircuit.ConductorSize} mm\u00b2 {nonSpaceCircuit.ConductorType}";
                scheduleSheet.InitRange(conductorDisplay, colI, row);
                colI += 1;
                
                var groundDisplay = nonSpaceCircuit.GroundingSize.HasError
                    ? string.Empty
                    : $"{NonSpaceCircuit.GroundingWireCount}-{nonSpaceCircuit.GroundingSize} mm\u00b2 {nonSpaceCircuit.Grounding}";
                scheduleSheet.InitRange(groundDisplay, colI, row);
                colI += 1;
                
                var racewayDisplay = nonSpaceCircuit.RacewaySize.HasError
                    ? string.Empty
                    : nonSpaceCircuit.RacewayTextDisplay;
                scheduleSheet.InitRange(racewayDisplay, colI, row);

                if (fixtureCircuit is not null && fixtureCircuit.IsItemized)
                {
                    foreach (var fixture in fixtureCircuit.Fixtures)
                    {
                        row += 1;
                        colI = 1;
                        scheduleSheet.InitRange(fixture.Description, colI, row, null, null, false);
                        scheduleSheet.GetRange(colI, row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        scheduleSheet.GetRange(colI, row).Style.Font.Italic = true;
                        colI += 1;
                        
                        scheduleSheet.InitRange(fixture.Quantity.ToString(), colI, row);
                        colI += 1;

                        var remainingFixtureCols = threePhaseBoard is null ? 14 : 17;
                        for (var i = 0; i < remainingFixtureCols; i++)
                        {
                            scheduleSheet.InitRange(string.Empty, colI, row);
                            colI += 1;
                        }
                    }

                    scheduleSheet.GetRange(1, row).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                } 
                else if (convenienceCircuit is not null)
                {
                    var convenienceCircuitValues = new List<(string, int)>();
                    if (convenienceCircuit.GfciReceptacleQuantity > 0)
                    {
                        var gfciDescription =
                            $"GFCI Receptacle ({(convenienceCircuit.AmpereTrip.HasError ? "" : convenienceCircuit.AmpereTrip.Value)} AT)";
                        convenienceCircuitValues.Add((gfciDescription, convenienceCircuit.GfciReceptacleQuantity));
                    }

                    if (convenienceCircuit.OneGangQuantity > 0)
                    {
                        convenienceCircuitValues.Add(("1-Gang", convenienceCircuit.OneGangQuantity));
                        
                    }

                    if (convenienceCircuit.TwoGangQuantity > 0)
                    {
                        convenienceCircuitValues.Add(("2-Gang", convenienceCircuit.TwoGangQuantity));
                    }
                    
                    if (convenienceCircuit.ThreeGangQuantity > 0)
                    {
                        convenienceCircuitValues.Add(("3-Gang", convenienceCircuit.ThreeGangQuantity));
                    }
                    
                    if (convenienceCircuit.FourGangQuantity > 0)
                    {
                        convenienceCircuitValues.Add(("4-Gang", convenienceCircuit.FourGangQuantity));
                    }

                    foreach (var (desc, qty) in convenienceCircuitValues)
                    {
                        row += 1;
                        colI = 1;
                        scheduleSheet.InitRange(desc, colI, row, null, null, false);
                        scheduleSheet.GetRange(colI, row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        scheduleSheet.GetRange(colI, row).Style.Font.Italic = true;
                        colI += 1;
                        
                        scheduleSheet.InitRange(qty.ToString(), colI, row);
                        colI += 1;
                        
                        var remainingConvenienceCols = threePhaseBoard is null ? 14 : 17;
                        
                        for (var i = 0; i < remainingConvenienceCols; i++)
                        {
                            scheduleSheet.InitRange(string.Empty, colI, row);
                            colI += 1;
                        }
                    }
                    
                    scheduleSheet.GetRange(1, row).Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
            }
            else if (subBoard is not null)
            {
                scheduleSheet.InitRange(subBoard.BoardName, colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(string.Empty, colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(subBoard.VoltAmpere.ToRoundedString(), colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(((int) subBoard.Voltage).ToString(), colI, row);
                colI += 1;
                
                string subBoardAmpereLoadDisplay = subBoard.AmpereLoad.HasError
                    ? string.Empty
                    : subBoard.AmpereLoad.Value.ToRoundedString();

                if (threePhaseBoard is null)
                {
                    scheduleSheet.InitRange(subBoardAmpereLoadDisplay, colI, row);
                }
                else
                {
                    if (subBoard is ThreePhaseDistributionBoard subThreePhaseBoard)
                    {
                        var subBoardAmpereLoadADisplay = subThreePhaseBoard.AmpereLoadA == 0
                            ? string.Empty
                            : subThreePhaseBoard.AmpereLoadA.ToRoundedString();
                        scheduleSheet.InitRange(subBoardAmpereLoadADisplay, colI, row);
                        colI += 1;
                        
                        var subBoardAmpereLoadBDisplay = subThreePhaseBoard.AmpereLoadB == 0
                            ? string.Empty
                            : subThreePhaseBoard.AmpereLoadB.ToRoundedString();
                        scheduleSheet.InitRange(subBoardAmpereLoadBDisplay, colI, row);
                        colI += 1;
                        
                        var subBoardAmpereLoadCDisplay = subThreePhaseBoard.AmpereLoadC == 0
                            ? string.Empty
                            : subThreePhaseBoard.AmpereLoadC.ToRoundedString();
                        scheduleSheet.InitRange(subBoardAmpereLoadCDisplay, colI, row);
                        colI += 1;
                        
                        var subBoardAmpereLoadAbcDisplay = subThreePhaseBoard.AmpereLoadAbc == 0
                            ? string.Empty
                            : subThreePhaseBoard.AmpereLoadAbc.ToRoundedString();
                        scheduleSheet.InitRange(subBoardAmpereLoadAbcDisplay, colI, row);
                    }
                    else
                    {
                        var boardAmpereLoadADisplay = subBoard.LineToLineVoltage == LineToLineVoltage.A
                            ? subBoardAmpereLoadDisplay
                            : string.Empty;
                        scheduleSheet.InitRange(boardAmpereLoadADisplay, colI, row);
                        colI += 1;
                        
                        var boardAmpereLoadBDisplay = subBoard.LineToLineVoltage == LineToLineVoltage.B
                            ? subBoardAmpereLoadDisplay
                            : string.Empty;
                        scheduleSheet.InitRange(boardAmpereLoadBDisplay, colI, row);
                        colI += 1;
                        
                        var boardAmpereLoadCDisplay = subBoard.LineToLineVoltage == LineToLineVoltage.C
                            ? subBoardAmpereLoadDisplay
                            : string.Empty;
                        scheduleSheet.InitRange(boardAmpereLoadCDisplay, colI, row);
                        colI += 1;
                        
                        scheduleSheet.InitRange(string.Empty, colI, row);
                    }
                }
                
                colI += 1;
                
                var ampereTripDisplay = subBoard.AmpereTrip.HasError
                    ? string.Empty
                    : subBoard.AmpereTrip.Value.ToString();
                scheduleSheet.InitRange(ampereTripDisplay, colI, row);
                colI += 1;
                
                var ampereFrameDisplay = subBoard.AmpereFrame.HasError
                    ? string.Empty
                    : subBoard.AmpereFrame.Value.ToString();
                scheduleSheet.InitRange(ampereFrameDisplay, colI, row);
                colI += 1;

                var phaseInt = subBoard.Phase == BoardPhase.SinglePhase ? 1 : 3;
                scheduleSheet.InitRange(phaseInt.ToString(), colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(subBoard.Pole.ToString(), colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(subBoard.WireLength.ToRoundedString(), colI, row);
                colI += 1;
                
                var voltageDropDisplay = subBoard.VoltageDrop.HasError
                    ? string.Empty
                    : subBoard.VoltageDrop.Value.ToRoundedString();
                scheduleSheet.InitRange(voltageDropDisplay, colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(subBoard.CircuitProtection.GetDisplayName(), colI, row);
                colI += 1;
                
                scheduleSheet.InitRange(subBoard.SetCount.ToString(), colI, row);
                colI += 1;
                
                var conductorDisplay = subBoard.ConductorSize.HasError 
                    ? string.Empty
                    : $"{subBoard.ConductorWireCount}-{subBoard.ConductorSize} mm\u00b2 {subBoard.ConductorType}";
                scheduleSheet.InitRange(conductorDisplay, colI, row);
                colI += 1;
                
                var groundDisplay = subBoard.GroundingSize.HasError
                    ? string.Empty
                    : $"{DistributionBoard.GroundingWireCount}-{subBoard.GroundingSize} mm\u00b2 {subBoard.Grounding}";
                scheduleSheet.InitRange(groundDisplay, colI, row);
                colI += 1;
                
                var racewayDisplay = subBoard.RacewaySize.HasError
                    ? string.Empty
                    : subBoard.RacewayTextDisplay;
                scheduleSheet.InitRange(racewayDisplay, colI, row);                
            }
            
            row += 1;
        }
        
        // TABLE FOOTER ---------------------------------------------------
        colI = 0;
        
        scheduleSheet.InitRange(string.Empty, colI, row);
        colI += 1;
        
        scheduleSheet.InitRange("Total", colI, row);
        colI += 1;
        
        scheduleSheet.InitRange(string.Empty, colI, row);
        colI += 1;
        
        scheduleSheet.InitRange(board.VoltAmpere.ToRoundedString(), colI, row);
        colI += 1;
        
        scheduleSheet.InitRange(string.Empty, colI, row);
        colI += 1;

        if (threePhaseBoard is not null)
        {
            scheduleSheet.InitRange(threePhaseBoard.AmpereLoadA.ToRoundedString(), colI, row);
            colI += 1;
            
            scheduleSheet.InitRange(threePhaseBoard.AmpereLoadB.ToRoundedString(), colI, row);
            colI += 1;
            
            scheduleSheet.InitRange(threePhaseBoard.AmpereLoadC.ToRoundedString(), colI, row);
            colI += 1;
            
            scheduleSheet.InitRange(threePhaseBoard.AmpereLoadAbc.ToRoundedString(), colI, row);
        }
        else
        {
            var boardAmpereLoadDisplay = board.AmpereLoad.HasError
                ? string.Empty
                : board.AmpereLoad.Value.ToRoundedString();
            scheduleSheet.InitRange(boardAmpereLoadDisplay, colI, row);
        }

        colI += 1;

        for (var i = 0; i < 11; i++)
        {
            scheduleSheet.InitRange(string.Empty, colI, row);
            colI += 1;
        }
    }

    private void _createComputationsSheet(ExcelPackage? package, DistributionBoard board)
    {
        var compSheet = package!.Workbook.Worksheets.Add($"{board.BoardName}: Com");
        var threePhaseBoard = board as ThreePhaseDistributionBoard;
        
        // PROJECT & BOARD DESCRIPTION -------------------------------------
        compSheet.Cells["A1:E1"].Merge = true;
        compSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        compSheet.Cells["A1"].Value = $"PROJECT NAME: {ProjectName}";
        
        compSheet.Cells["F1:J1"].Merge = true;
        compSheet.Cells["F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        var configText = threePhaseBoard is null
            ? ""
            : " " + threePhaseBoard.ThreePhaseConfiguration.GetDisplayName();
        compSheet.Cells["F1"].Value =
            $"SYSTEM: {board.Phase.GetDisplayName()}{configText}";
            
        compSheet.Cells["A2:E2"].Merge = true;
        compSheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        compSheet.Cells["A2"].Value = $"DISTRIBUTION BOARD NAME: {board.BoardName}";
            
        compSheet.Cells["F2:J2"].Merge = true;
        compSheet.Cells["F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
        compSheet.Cells["F2"].Value = $"VOLTAGE: {(int) board.Voltage} V";
        
        // COMPUTATIONS ----------------------------------------------------
        
        var colI = 0;
        var row = 6;

        var childLightingOutlets =
            board
                .Circuits.OfType<LightingOutletCircuit>()
                .Where(c => !c.VoltAmpere.HasError).ToList();

        var childConvenienceOutlets =
            board
                .Circuits.OfType<ConvenienceOutletCircuit>()
                .Where(c => !c.VoltAmpere.HasError).ToList();
        
        var childMotorOutlets =
            board
                .Circuits.OfType<MotorOutletCircuit>()
                .Where(c => !c.VoltAmpere.HasError).ToList();
        
        var childApplianceOutlets =
            board
                .Circuits.OfType<ApplianceEquipmentOutletCircuit>()
                .Where(c => !c.VoltAmpere.HasError);

        var childSpareOutlets =
            board.Circuits.OfType<SpareCircuit>().ToList();
        
        var lightingOutletsVoltAmpere = board.FilterVoltAmpere<LightingOutletCircuit>();
        var convenienceOutletsVoltAmpere = board.FilterVoltAmpere<ConvenienceOutletCircuit>();
        var motorOutletsVoltAmpere = board.FilterVoltAmpere<MotorOutletCircuit>();
        var applianceOutletsVoltAmpere = board.FilterVoltAmpere<ApplianceEquipmentOutletCircuit>();
        var spareVoltAmpere = board.FilterVoltAmpere<SpareCircuit>();

        if (board.BuildingClassification == BuildingClassification.DwellingUnit)
        {
            compSheet.InitCompCell("Lighting and Convenience Outlets", colI, row, 20, true, true);
            row += 1;
            
            // TODO: Add computations for Lighting and Convenience Outlets
            var lightingAndConvenienceOutletsVoltAmpere =
                lightingOutletsVoltAmpere +
                convenienceOutletsVoltAmpere;

            var subBoardsWithLightingAndConvenienceOutlets =
                board
                    .SubDistributionBoards
                    .Where(b =>
                        b.FilterVoltAmpere<LightingOutletCircuit>() > 0 ||
                        b.FilterVoltAmpere<ConvenienceOutletCircuit>() > 0
                    );

            if (lightingAndConvenienceOutletsVoltAmpere == 0)
            {
                compSheet.InitCompCell("None", colI, row);
            }
            else
            {
                foreach (var lightingOutlet in childLightingOutlets)
                {
                    var text = $"- {lightingOutlet.Description} = {lightingOutlet.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }

                foreach (var convenienceOutlet in childConvenienceOutlets)
                {
                    var text = $"- {convenienceOutlet.Description} = {convenienceOutlet.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }

                foreach (var subBoard in subBoardsWithLightingAndConvenienceOutlets)
                {
                    var subBoardLightingOutletsVoltAmpere = subBoard.FilterVoltAmpere<LightingOutletCircuit>();
                    var subBoardConvenienceOutletsVoltAmpere = subBoard.FilterVoltAmpere<ConvenienceOutletCircuit>();
                    var text =
                        $"Sub Total ({subBoard.BoardName}) = {(subBoardLightingOutletsVoltAmpere + subBoardConvenienceOutletsVoltAmpere).ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }

                var subTotalText = $"Sub Total = {lightingAndConvenienceOutletsVoltAmpere}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;
                
                var lightingAndConvenienceCircuitsVoltAmpereRanges = 
                    DemandFactorFormulas.ApplyDemandFactorToDwellingUnitLightingAndConvenienceCircuits(
                        lightingAndConvenienceOutletsVoltAmpere
                    );
                    
                var firstRangeValue = lightingAndConvenienceCircuitsVoltAmpereRanges[0];
                var secondRangeValue = lightingAndConvenienceCircuitsVoltAmpereRanges[1];
                var thirdRangeValue = lightingAndConvenienceCircuitsVoltAmpereRanges[2];

                var firstRangeText = $"First 3000 VA @ 100% = {firstRangeValue.ToRoundedString()}";
                compSheet.InitCompCell(firstRangeText, colI, row);
                row += 1;

                if (secondRangeValue > 0)
                {
                    var secondRangeText = $"From 3001 to 120000 VA @ 35% = {secondRangeValue.ToRoundedString()}";
                    compSheet.InitCompCell(secondRangeText, colI, row);
                    row += 1;
                }

                if (thirdRangeValue > 0)
                {
                    var thirdRangeText = $"Remainder over 120000 VA @ 25% = {thirdRangeValue.ToRoundedString()}";
                    compSheet.InitCompCell(thirdRangeText, colI, row);
                    row += 1;
                }

                row += 1;
                var finalTotalText = $"Final total = {firstRangeText + secondRangeValue + thirdRangeValue}VA";
                compSheet.InitCompCell(finalTotalText, colI, row, 14, true);
                row += 1;
                
                var referenceText =
                    "Reference: Lighting load Demand Factors, Philippine Electrical Code Part 1, Chap. 2.20, no. 2.20.3.3, pp. 56, 2017.";
                compSheet.InitCompCell(referenceText, colI, row);
            }
        }
        else
        {
            compSheet.InitCompCell("Lighting Outlets", colI, row, 20, true, true);
            row += 1;

            if (lightingOutletsVoltAmpere == 0)
            {
                compSheet.InitCompCell("None", colI, row);
            }
            else
            {
                foreach (var lightingOutlet in childLightingOutlets)
                {
                    var text = $"- {lightingOutlet.Description} = {lightingOutlet.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subBoardsWithLightingOutlets =
                    board
                        .SubDistributionBoards
                        .Where(b => b.FilterVoltAmpere<LightingOutletCircuit>() > 0);

                foreach (var subBoard in subBoardsWithLightingOutlets)
                {
                    var subBoardLightingOutletsVoltAmpere = subBoard.FilterVoltAmpere<LightingOutletCircuit>();
                    var text =
                        $"Sub Total ({subBoard.BoardName}) = {subBoardLightingOutletsVoltAmpere.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subTotalText = $"Sub Total = {lightingOutletsVoltAmpere}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;

                double nonDwellingLightingOutletsVoltAmpereWithDemandFactor;
                
                if (board.BuildingClassification == BuildingClassification.Hospital)
                {
                    var hospitalLightingOutletsVoltAmpereRanges =
                        DemandFactorFormulas.ApplyDemandFactorToHospitalLightingCircuits(
                            lightingOutletsVoltAmpere
                        );
                    var firstHospitalLightingRangeValue = hospitalLightingOutletsVoltAmpereRanges[0];
                    var secondHospitalLightingRangeValue = hospitalLightingOutletsVoltAmpereRanges[1];

                    var firstRangeText =
                        $"First 50000 @ 40% = {firstHospitalLightingRangeValue.ToRoundedString()}VA";
                    compSheet.InitCompCell(firstRangeText, colI, row);
                    row += 1;

                    if (secondHospitalLightingRangeValue > 0)
                    {
                        var secondRangeText =
                            $"Remainder over 50000 @ 20% = {secondHospitalLightingRangeValue.ToRoundedString()}VA";
                        compSheet.InitCompCell(secondRangeText, colI, row);
                        row += 1;
                    }
                    
                    nonDwellingLightingOutletsVoltAmpereWithDemandFactor = 
                        firstHospitalLightingRangeValue + secondHospitalLightingRangeValue;
                } 
                else if (board.BuildingClassification == BuildingClassification.HotelMotelApartment)
                {
                    var hotelLightingOutletsVoltAmpereRanges =
                        DemandFactorFormulas.ApplyDemandFactorToHotelMotelApartmentLightingCircuits(
                            lightingOutletsVoltAmpere
                        );
                    var firstHotelLightingRangeValue = hotelLightingOutletsVoltAmpereRanges[0];
                    var secondHotelLightingRangeValue = hotelLightingOutletsVoltAmpereRanges[1];
                    var thirdHotelLightingRangeValue = hotelLightingOutletsVoltAmpereRanges[2];

                    var firstRangeText =
                        $"First 20000 @ 50% = {firstHotelLightingRangeValue.ToRoundedString()}VA";
                    compSheet.InitCompCell(firstRangeText, colI, row);
                    row += 1;

                    if (secondHotelLightingRangeValue > 0)
                    {
                        var secondRangeText =
                            $"From 20001 to 100000 @ 40% = {secondHotelLightingRangeValue.ToRoundedString()}VA";
                        compSheet.InitCompCell(secondRangeText, colI, row);
                        row += 1;
                    }

                    if (thirdHotelLightingRangeValue > 0)
                    {
                        var thirdRangeText =
                            $"Remainder over 100000 @ 30% = {thirdHotelLightingRangeValue.ToRoundedString()}VA";
                        compSheet.InitCompCell(thirdRangeText, colI, row);
                        row += 1;
                    }
                    
                    nonDwellingLightingOutletsVoltAmpereWithDemandFactor = 
                        firstHotelLightingRangeValue + 
                        secondHotelLightingRangeValue + 
                        thirdHotelLightingRangeValue;
                }
                else if (board.BuildingClassification == BuildingClassification.Warehouse)
                {
                    var warehouseOutletsVoltAmpereRanges =
                        DemandFactorFormulas.ApplyDemandFactorToWarehouseLightingCircuits(
                            lightingOutletsVoltAmpere
                        );
                    var firstWarehouseLightingRangeValue = warehouseOutletsVoltAmpereRanges[0];
                    var secondWarehouseLightingRangeValue = warehouseOutletsVoltAmpereRanges[1];

                    var firstRangeText =
                        $"First 12500 @ 100% = {firstWarehouseLightingRangeValue.ToRoundedString()}";
                    compSheet.InitCompCell(firstRangeText, colI, row);
                    row += 1;

                    if (secondWarehouseLightingRangeValue > 0)
                    {
                        var secondRangeText =
                            $"Remainder over 12500 @ 50% = {secondWarehouseLightingRangeValue.ToRoundedString()}";
                        compSheet.InitCompCell(secondRangeText, colI, row);
                        row += 1;
                    }
                    
                    nonDwellingLightingOutletsVoltAmpereWithDemandFactor = 
                        firstWarehouseLightingRangeValue + 
                        secondWarehouseLightingRangeValue;
                }
                else
                {
                    nonDwellingLightingOutletsVoltAmpereWithDemandFactor = lightingOutletsVoltAmpere;
                }

                var finalTotalText =
                    $"Final total = {nonDwellingLightingOutletsVoltAmpereWithDemandFactor.ToRoundedString()}VA";
                compSheet.InitCompCell(finalTotalText, colI, row, 14, true);
                row += 1;

                var referenceText =
                    "Reference: Lighting load Demand Factors, Philippine Electrical Code Part 1, Chap. 2.20, no. 2.20.3.3, pp. 56, 2017.";
                compSheet.InitCompCell(referenceText, colI, row);
                row += 1;
            }

            row += 1;
            compSheet.InitCompCell("Convenience Outlets", colI, row, 20, true, true);
            row += 1;

            if (convenienceOutletsVoltAmpere == 0)
            {
                compSheet.InitCompCell("None", colI, row);
            }
            else
            {
                foreach (var convenienceOutlet in childConvenienceOutlets)
                {
                    var text = $"- {convenienceOutlet.Description} = {convenienceOutlet.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subBoardsWithConvenienceOutlets =
                    board
                        .SubDistributionBoards
                        .Where(b => b.FilterVoltAmpere<LightingOutletCircuit>() > 0);

                foreach (var subBoard in subBoardsWithConvenienceOutlets)
                {
                    var subBoardConvenienceOutletsVoltAmpere = subBoard.FilterVoltAmpere<ConvenienceOutletCircuit>();
                    var text =
                        $"Sub Total ({subBoard.BoardName}) = {subBoardConvenienceOutletsVoltAmpere.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subTotalText = $"Sub Total = {convenienceOutletsVoltAmpere}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;
                
                var nonDwellingConvenienceOutletsVoltAmpereRanges =
                    DemandFactorFormulas.ApplyDemandFactorToNonDwellingConvenienceCircuits(
                        convenienceOutletsVoltAmpere
                    );
                var firstNonDwellingConvenienceRangeValue = nonDwellingConvenienceOutletsVoltAmpereRanges[0];
                var secondNonDwellingConvenienceRangeValue = nonDwellingConvenienceOutletsVoltAmpereRanges[1];

                var firstRangeText =
                    $"First 10000 @ 100% = {firstNonDwellingConvenienceRangeValue.ToRoundedString()}VA";
                compSheet.InitCompCell(firstRangeText, colI, row);
                row += 1;

                if (secondNonDwellingConvenienceRangeValue > 0)
                {
                    var secondRangeText =
                        $"Remainder over 10000 @@ 50% = {secondNonDwellingConvenienceRangeValue.ToRoundedString()}";
                    compSheet.InitCompCell(secondRangeText, colI, row);
                    row += 1;
                }
                
                var finalTotalText =
                    $"Final total = {nonDwellingConvenienceOutletsVoltAmpereRanges.Sum().ToRoundedString()}VA";
                compSheet.InitCompCell(finalTotalText, colI, row, 14, true);
                row += 1;

                var referenceText =
                    "Reference: Demand Factors for Non-Dwelling Receptacle Loads, Philippine Electrical Code Part 1, Chap. 2.20, no. 2.20.3.5, pp. 56, 2017.";
                compSheet.InitCompCell(referenceText, colI, row);
            }
        }
        
        row += 2;
        
        compSheet.InitCompCell("Motor Outlets", colI, row, 20, true, true);
        row += 1;
        
        if (motorOutletsVoltAmpere == 0)
        {
            compSheet.InitCompCell("None", colI, row);
        }
        else
        {
            var normalMotorsVoltAmpere =
                board.FilterVoltAmpere<MotorOutletCircuit>(
                    mc => mc.MotorApplication == MotorApplication.NormalMotor
                );
            
            var feedersVoltAmpere =
                board.FilterVoltAmpere<MotorOutletCircuit>(
                    mc => mc.MotorApplication == MotorApplication.ElevatorFeeder
                );
            
            var feeders =
                board.FilterNestedCircuits<MotorOutletCircuit>(m =>
                    m.MotorApplication == MotorApplication.ElevatorFeeder
                );

            var feedersVoltAmpereWithDemandFactor =
                DemandFactorFormulas.ApplyDemandFactorToElevatorFeeders(feeders);
            
            var cranesAndHoistsVoltAmpere =
                board.FilterVoltAmpere<MotorOutletCircuit>(
                    mc => mc.MotorApplication == MotorApplication.CranesAndHoist
                );
            
            var cranes =
                board.FilterNestedCircuits<MotorOutletCircuit>(m =>
                    m.MotorApplication == MotorApplication.CranesAndHoist
                );

            var cranesVoltAmpereWithDemandFactor =
                DemandFactorFormulas.ApplyDemandFactorToCranesAndHoists(cranes);
            

            if (normalMotorsVoltAmpere > 0)
            {
                var childNormalMotors =
                    childMotorOutlets.Where(m => m.MotorApplication == MotorApplication.NormalMotor);
                
                var subBoardsWithNormalMotors =
                    board
                        .SubDistributionBoards
                        .Where(b =>
                            b.FilterVoltAmpere<MotorOutletCircuit>(
                                mc => mc.MotorApplication == MotorApplication.NormalMotor
                            ) > 0
                        );
                
                compSheet.InitCompCell("For Normal Motors", colI, row, 16, true);
                row += 1;

                foreach (var normalMotor in childNormalMotors)
                {
                    var text =
                        $"- {normalMotor.Description} = {normalMotor.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }

                foreach (var subBoard in subBoardsWithNormalMotors)
                {
                    var subBoardNormalMotorsVoltAmpere = subBoard
                        .FilterVoltAmpere<MotorOutletCircuit>(
                            mc => mc.MotorApplication == MotorApplication.NormalMotor);
                    var text =
                        $"Sub Total ({subBoard.BoardName}) = {subBoardNormalMotorsVoltAmpere.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subTotalText = $"Sub Total = {normalMotorsVoltAmpere.ToRoundedString()}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;
                
                compSheet.InitCompCell("No Demand Factors Applied", colI, row);
                row += 1;
            }

            if (feedersVoltAmpere > 0)
            {
                
                var childFeeders =
                    childMotorOutlets.Where(m => m.MotorApplication == MotorApplication.ElevatorFeeder);
                
                var subBoardsWithFeeders =
                    board
                        .SubDistributionBoards
                        .Where(b =>
                            b.FilterVoltAmpere<MotorOutletCircuit>(
                                mc => mc.MotorApplication == MotorApplication.ElevatorFeeder
                            ) > 0
                        );
                
                compSheet.InitCompCell("For Elevator Feeders", colI, row, 16, true);
                row += 1;

                foreach (var feeder in childFeeders)
                {
                    var text =
                        $"- {feeder.Description} = {feeder.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }

                foreach (var subBoard in subBoardsWithFeeders)
                {
                    var subBoardFeedersVoltAmpere = subBoard
                        .FilterVoltAmpere<MotorOutletCircuit>(
                            mc => mc.MotorApplication == MotorApplication.ElevatorFeeder);
                    var text =
                        $"Sub Total ({subBoard.BoardName}) = {subBoardFeedersVoltAmpere.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subTotalText = $"Sub Total = {feedersVoltAmpere.ToRoundedString()}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;
                
                var subTotalDfText = $"Sub Total with Demand Factor for Elevator Feeders = {feedersVoltAmpereWithDemandFactor.ToRoundedString()}VA";
                compSheet.InitCompCell(subTotalDfText, colI, row);
                row += 1;

                var referenceText =
                    "“Demand Factors for Elevators”, Philippine Electrical Code Part 1, Chap. 6.20, no. 6.20.2.4, pp. 600, 2017";
                compSheet.InitCompCell(referenceText, colI, row);
                row += 1;
            }

            if (cranesAndHoistsVoltAmpere > 0)
            {
                var childCranes =
                    childMotorOutlets.Where(m => m.MotorApplication == MotorApplication.CranesAndHoist);
                
                var subBoardsWithCranes =
                    board
                        .SubDistributionBoards
                        .Where(b =>
                            b.FilterVoltAmpere<MotorOutletCircuit>(
                                mc => mc.MotorApplication == MotorApplication.CranesAndHoist
                            ) > 0
                        );
                
                compSheet.InitCompCell("For Cranes and Hoists", colI, row, 16, true);
                row += 1;

                foreach (var crane in childCranes)
                {
                    var text =
                        $"- {crane.Description} = {crane.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }

                foreach (var subBoard in subBoardsWithCranes)
                {
                    var subBoardCranesVoltAmpere = subBoard
                        .FilterVoltAmpere<MotorOutletCircuit>(
                            mc => mc.MotorApplication == MotorApplication.CranesAndHoist);
                    var text =
                        $"Sub Total ({subBoard.BoardName}) = {subBoardCranesVoltAmpere.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subTotalText = $"Sub Total = {cranesAndHoistsVoltAmpere.ToRoundedString()}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;
                
                var subTotalDfText = $"Sub Total with Demand Factor for Elevator Feeders = {cranesVoltAmpereWithDemandFactor.ToRoundedString()}VA";
                compSheet.InitCompCell(subTotalDfText, colI, row);
                row += 1;

                var referenceText =
                    $"“Demand Factors for Cranes and Hoist”, Philippine Electrical Code Part 1, Chap. 6.10, no. 6.10.2.4(E), pp. 594, 2017";
                compSheet.InitCompCell(referenceText, colI, row);
                row += 1;
            }

            var finalTotalText = $"Final total = {(normalMotorsVoltAmpere + feedersVoltAmpereWithDemandFactor + cranesVoltAmpereWithDemandFactor).ToRoundedString()}VA";
            compSheet.InitCompCell(finalTotalText, colI, row, 14, true);
        }

        row += 2;
        
        compSheet.InitCompCell("Appliance/Equipment Outlets", colI, row, 20, true, true);
        row += 1;

        if (applianceOutletsVoltAmpere == 0)
        {
            compSheet.InitCompCell("None", colI, row);
        }
        else
        {
            var applianceEquipmentVoltAmpere =
                board.FilterVoltAmpere<ApplianceEquipmentOutletCircuit>();
            
            var dryersVoltAmpere = 
                board.FilterVoltAmpere<ApplianceEquipmentOutletCircuit>(
                    aec => aec.ApplianceType == ApplianceType.Dryer
                );

            var dryers =
                board.FilterNestedCircuits<ApplianceEquipmentOutletCircuit>(
                    aec => aec.ApplianceType == ApplianceType.Dryer && !aec.VoltAmpere.HasError
                );
            
            var dryersVoltAmpereWithDemandFactor = 
                DemandFactorFormulas.ApplyDemandFactorToDryers(dryers);
            
            var kitchenEquipmentsVoltAmpere =
                board.FilterVoltAmpere<ApplianceEquipmentOutletCircuit>(
                    aec => aec.ApplianceType == ApplianceType.KitchenEquipment
                );

            var kitchenEquipments =
                board.FilterNestedCircuits<ApplianceEquipmentOutletCircuit>(
                    aec => aec.ApplianceType == ApplianceType.KitchenEquipment && !aec.VoltAmpere.HasError
                );
            
            var dwellingUnitKitchenEquipmentsVoltAmpereRange =
                DemandFactorFormulas.ApplyDemandFactorToDwellingUnitKitchenEquipment(kitchenEquipments);
            
            var dwellingUnitKitchenEquipmentsVoltAmpereWithDemandFactor =
                dwellingUnitKitchenEquipmentsVoltAmpereRange[0] + dwellingUnitKitchenEquipmentsVoltAmpereRange[1];
            
            var nonDwellingUnitKitchenEquipmentsVoltAmpereWithDemandFactor =
                DemandFactorFormulas.ApplyDemandFactorToNonDwellingUnitKitchenEquipment(kitchenEquipments);
            
            var otherApplianceEquipmentsVoltAmpere = 
                board.FilterVoltAmpere<ApplianceEquipmentOutletCircuit>(
                    aec => aec.ApplianceType == ApplianceType.Other
                );
            
            var otherApplianceEquipments = 
                childApplianceOutlets.Where(
                    aec => aec.ApplianceType == ApplianceType.Other && !aec.VoltAmpere.HasError
                );
            
            var subBoardsWithOtherApplianceEquipments = 
                board.SubDistributionBoards.Where(
                    b => b.FilterVoltAmpere<ApplianceEquipmentOutletCircuit>(
                        aec => aec.ApplianceType == ApplianceType.Other
                    ) > 0
                );

            var applianceEquipmentVoltAmpereWithDemandFactor =
                dryersVoltAmpereWithDemandFactor +
                otherApplianceEquipmentsVoltAmpere;
            
            if (board.BuildingClassification == BuildingClassification.DwellingUnit)
            {
                applianceEquipmentVoltAmpereWithDemandFactor += dwellingUnitKitchenEquipmentsVoltAmpereWithDemandFactor;
            }
            else
            {
                applianceEquipmentVoltAmpereWithDemandFactor += nonDwellingUnitKitchenEquipmentsVoltAmpereWithDemandFactor;
            }

            if (dryersVoltAmpere > 0)
            {
                compSheet.InitCompCell("For Dryers", colI, row, 16, true);
                row += 1;
                
                foreach (var dryer in dryers)
                {
                    var text =
                        $"- {dryer.Description} = {dryer.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subTotalText = $"Sub Total = {dryersVoltAmpere.ToRoundedString()}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;
                
                var subTotalDfText = $"Sub Total with Demand Factor for Dryers = {dryersVoltAmpereWithDemandFactor.ToRoundedString()}VA";
                compSheet.InitCompCell(subTotalDfText, colI, row);
                row += 1;

                var referenceText =
                    "“Demand Factors for Household Electric Clothes Dryers”, Philippine Electrical Code Part 1, Chap. 2.20, no. 2.20.3.15, pp. 57, 2017";
                compSheet.InitCompCell(referenceText, colI, row);
                row += 1;
            }

            if (kitchenEquipmentsVoltAmpere > 0)
            {
                compSheet.InitCompCell("For Kitchen Equipment", colI, row, 16, true);
                row += 1;

                if (board.BuildingClassification == BuildingClassification.DwellingUnit)
                {
                    var kitchenEquipmentsLessThan3500VaTotalVoltAmpere = dwellingUnitKitchenEquipmentsVoltAmpereRange[0];
                    var kitchenEquipmentsGreaterThan3500VaTotalVoltAmpere = dwellingUnitKitchenEquipmentsVoltAmpereRange[1];
                    
                    var kitchenEquipmentsLessThan3500Va =
                        kitchenEquipments.Where(ke => ke.VoltAmpere.Value < 3500 && !ke.VoltAmpere.HasError).ToList();
                        
                    var kitchenEquipmentsGreaterThan3500Va =
                        kitchenEquipments.Where(ke => ke.VoltAmpere.Value >= 3500 && !ke.VoltAmpere.HasError).ToList();

                    if (kitchenEquipmentsLessThan3500VaTotalVoltAmpere > 0)
                    {
                        var keLessThan3500VaSum =
                            kitchenEquipmentsLessThan3500Va.Select(ke => ke.VoltAmpere.Value).Sum();
                        
                        foreach (var kitchenEquipment in kitchenEquipmentsLessThan3500Va)
                        {
                            var text =
                                $"- {kitchenEquipment.Description} = {kitchenEquipment.VoltAmpere.Value.ToRoundedString()}VA";
                            compSheet.InitCompCell(text, colI, row);
                            row += 1;
                        }
                        
                        var subTotalText = $"Sub Total = {keLessThan3500VaSum.ToRoundedString()}VA";
                        compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                        row += 1;

                        var subTotalDfText =
                            $"Sub Total with Demand Factor for Dwelling Unit Kitchen Equipment with Less than 3500 VA = {kitchenEquipmentsLessThan3500VaTotalVoltAmpere.ToRoundedString()}";
                        compSheet.InitCompCell(subTotalDfText, colI, row);
                        row += 1;
                    }

                    if (kitchenEquipmentsGreaterThan3500VaTotalVoltAmpere > 0)
                    {
                        var keGreaterThan3500VaSum =
                            kitchenEquipmentsGreaterThan3500Va.Select(ke => ke.VoltAmpere.Value).Sum();
                        
                        foreach (var kitchenEquipment in kitchenEquipmentsGreaterThan3500Va)
                        {
                            var text =
                                $"- {kitchenEquipment.Description} = {kitchenEquipment.VoltAmpere.Value.ToRoundedString()}VA";
                            compSheet.InitCompCell(text, colI, row);
                            row += 1;
                        }
                        
                        var subTotalText = $"Sub Total = {keGreaterThan3500VaSum.ToRoundedString()}VA";
                        compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                        row += 1;

                        var subTotalDfText =
                            $"Sub Total with Demand Factor for Dwelling Unit Kitchen Equipment with Less than 3500 VA = {kitchenEquipmentsGreaterThan3500VaTotalVoltAmpere.ToRoundedString()}";
                        compSheet.InitCompCell(subTotalDfText, colI, row);
                        row += 1;
                    }

                    var referenceText =
                        "<p>Reference: “Demand Factors and Loads for Household Electric Ranges, Wall Mounted Ovens, Counter-Mounted Cooking Units, and Other Household Cooking Appliances over 1 \u00be kW”, Philippine Electrical Code Part 1, Chap. 2.20, no. 2.20.3.16, pp. 58, 2017.</p>";
                    compSheet.InitCompCell(referenceText, colI, row);
                }
                else
                {
                    foreach (var kitchenEquipment in kitchenEquipments)
                    {
                        var text =
                            $"- {kitchenEquipment.Description} = {kitchenEquipment.VoltAmpere.Value.ToRoundedString()}VA";
                        compSheet.InitCompCell(text, colI, row);
                        row += 1;
                    }

                    var subTotalText = $"Sub Total = {kitchenEquipmentsVoltAmpere.ToRoundedString()}";
                    compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                    row += 1;

                    var subTotalDfText =
                        $"Sub Total with Demand Factor for Non-Dwelling Unit Kitchen Equipment = {nonDwellingUnitKitchenEquipmentsVoltAmpereWithDemandFactor.ToRoundedString()}";
                    compSheet.InitCompCell(subTotalDfText, colI, row);
                    row += 1;

                    var referenceText =
                        "Reference: “Demand Factors for Kitchen Equipment – other than Dwelling Units”, Philippine Electrical Code Part 1, Chap. 2.20, no. 2.20.3.16, pp. 58, 2017.";
                    compSheet.InitCompCell(referenceText, colI, row);
                }
                row += 1;
            }

            if (otherApplianceEquipmentsVoltAmpere > 0)
            {
                compSheet.InitCompCell("For Other Equipment:", colI, row, 16, true);
                row += 1;
                
                foreach (var otherAppliance in otherApplianceEquipments)
                {
                    var text = $"- {otherAppliance.Description} = {otherAppliance.VoltAmpere.Value.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }

                foreach (var subBoard in subBoardsWithOtherApplianceEquipments)
                {
                    var subBoardOtherApplianceEquipmentsVoltAmpere = subBoard.FilterVoltAmpere<ApplianceEquipmentOutletCircuit>(
                        aec => aec.ApplianceType == ApplianceType.Other
                    );
                    
                    var text =
                        $"Sub Total ({subBoard.BoardName}) = {subBoardOtherApplianceEquipmentsVoltAmpere.ToRoundedString()}VA";
                    compSheet.InitCompCell(text, colI, row);
                    row += 1;
                }
                
                var subTotalText = $"Sub Total = {convenienceOutletsVoltAmpere}VA";
                compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
                row += 1;
                
                compSheet.InitCompCell("No Demand Factors Applied", colI, row);
                row += 1;
            }
            
            row += 1;
            var finalTotalText = $"Final total = {applianceEquipmentVoltAmpereWithDemandFactor.ToRoundedString()}VA";
            compSheet.InitCompCell(finalTotalText, colI, row, 14, true);
        }

        row += 2;
                
        compSheet.InitCompCell("Spare Outlets", colI, row, 20, true, true);
        row += 1;

        if (spareVoltAmpere == 0)
        {
            compSheet.InitCompCell("None", colI, row);
        }
        else
        {
            for (var i = 0; i < childSpareOutlets.Count; i++)
            {
                var spareOutlet = childSpareOutlets[i];
                
                var text = $"- Spare {i + 1} = {spareOutlet.VoltAmpere.ToRoundedString()}VA";
                compSheet.InitCompCell(text, colI, row);
                row += 1;
            }
            
            var subBoardsWithSpareOutlets =
                board
                    .SubDistributionBoards
                    .Where(b => b.FilterVoltAmpere<SpareCircuit>() > 0);

            foreach (var subBoard in subBoardsWithSpareOutlets)
            {
                var subBoardSpareOutletsVoltAmpere = subBoard.FilterVoltAmpere<SpareCircuit>();
                var text =
                    $"Sub Total ({subBoard.BoardName}) = {subBoardSpareOutletsVoltAmpere.ToRoundedString()}VA";
                compSheet.InitCompCell(text, colI, row);
                row += 1;
            }
            
            var subTotalText = $"Sub Total = {spareVoltAmpere.ToRoundedString()}VA";
            compSheet.InitCompCell(subTotalText, colI, row, null, false, true);
        }
        
        row += 2;
        
        compSheet.InitCompCell("Overall Total", colI, row, 20, true, true);
        row += 1;

        var vaText = $"Total VA (S) = {board.VoltAmpere.ToRoundedString()}";
        compSheet.InitCompCell(vaText, colI, row, 14, true);
        row += 1;
        
        // TODO: Show current
    }
}