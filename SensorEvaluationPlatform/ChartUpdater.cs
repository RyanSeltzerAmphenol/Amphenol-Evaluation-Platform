using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

public class ChartUpdaterEventArgs : EventArgs
{
    public int ChartNumber { get; }
    public string[] Parts { get; }

    public ChartUpdaterEventArgs(int chartNumber, string[] parts)
    {
        ChartNumber = chartNumber;
        Parts = parts;
    }
}

public class ChartUpdater
{
    public delegate void ChartUpdatedEventHandler(object sender, ChartUpdaterEventArgs e);
    public event ChartUpdatedEventHandler ChartUpdated;

    protected virtual void OnChartUpdated(ChartUpdaterEventArgs e)
    {
        ChartUpdated?.Invoke(this, e);
    }

    public async Task UpdateChartAsync(string input)
    {
        await Task.Run(() =>
        {
            string[] parts = input.Split(',');
            if (!IsValidInput(parts, out int chartNumber)) return;

            // If valid, raise the event with the chart number and parts
            OnChartUpdated(new ChartUpdaterEventArgs(chartNumber, parts));
        });
    }

    private bool IsValidInput(string[] parts, out int chartNumber)
    {
        chartNumber = 0;
        if (parts.Length < 2 || !int.TryParse(parts[0], out chartNumber))
        {
            // Log error or notify user
            return false;
        }

        // Add additional validation as necessary
        return true;
    }
}
