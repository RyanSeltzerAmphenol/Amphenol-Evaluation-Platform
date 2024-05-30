using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SensorEvaluationPlatform
{

    public partial class EvalPlatform : Form
    {
        #region Initialization & Closing
        /////////////////Global Variables///////////////
        List<string> connectedPorts = new List<string>();
        string selectedPort;
        string filePath;
        bool isStarted = false;
        bool isLogging = false;
        bool firstReadDevice1 = false;
        bool firstReadDevice2 = false;
        bool isAnimating = false;
        bool findAllCom = false;
        Point? prevPosition = null;
        ToolTip tooltip = new ToolTip();
        private CancellationTokenSource cancellationTokenSource;
        private Dictionary<int, string> lastKnownTitles = new Dictionary<int, string>();
        PictureBox pictureBox = new PictureBox();
        private readonly ChartUpdater chartUpdater;
        /////////////Rotating Icon Settings/////////////
        private bool isRotating = false;
        private System.Windows.Forms.Timer rotationTimer;
        private int rotationAngle = 0;
        private const int rotationSpeed = 10;
        /////////////Animated Chart Scaling/////////////
        private static System.Timers.Timer animationTimer = new System.Timers.Timer();
        private static object lockObj = new object();
        /////////////////////////////////////////////////
        public EvalPlatform()
        {
            InitializeComponent();
            InitializeRotationTimer();
            chartUpdater = new ChartUpdater();
            chartUpdater.ChartUpdated += ChartUpdater_ChartUpdated;
            chart1.MouseClick += new MouseEventHandler(chart1_MouseClick);
            chart2.MouseClick += new MouseEventHandler(chart2_MouseClick);
            ReplaceTabControlWithImage(true);
            tcGraphs.TabPages.Clear();
            tsbVer.Text = VersionLabel;
            DarkMode();
        }
        private void EvalPlatform_Load(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => FindRaspberryPiPicoCOMPortAsync());
        }
        private void InitializeRotationTimer()
        {
            rotationTimer = new System.Windows.Forms.Timer();
            rotationTimer.Interval = 10; // Change this value to adjust rotation smoothness
            rotationTimer.Tick += RotationTimer_Tick;
        }
        private void EvalPlatform_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(isStarted) { tsbStart.PerformClick(); }
            //rotationTimer.Stop();
            //animationTimer.Stop();
            Properties.Settings.Default.Save();
        }
        public string VersionLabel
        {
            get
            {
                if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                {
                    Version ver = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return string.Format("Version: {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision, Assembly.GetEntryAssembly().GetName().Name);
                }
                else
                {
                    var ver = Assembly.GetExecutingAssembly().GetName().Version;
                    return string.Format("Version: {0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision, Assembly.GetEntryAssembly().GetName().Name);
                }
            }
        }
        
        public void DarkMode()
        {
            if (!Properties.Settings.Default.DarkMode)
            {
                //Light mode
                tableLayoutPanel1.BackColor = SystemColors.Control;
                tableLayoutPanel2.BackColor = SystemColors.ButtonFace;
                rtbConsole.BackColor = SystemColors.ButtonFace;
                chart1.BackColor = Color.White;
                chart2.BackColor = Color.White;
                cbConnected.ForeColor = SystemColors.ControlText;
                lblComPort.ForeColor = SystemColors.ControlText;
                tsbDarkMode.Image = Properties.Resources.light_mode;
                pictureBox.Image = Properties.Resources.Background;
                Properties.Settings.Default.DarkMode = false;
            }
            else
            {
                //Dark mode
                tableLayoutPanel1.BackColor = SystemColors.ControlDark;
                tableLayoutPanel2.BackColor = SystemColors.ActiveBorder;
                rtbConsole.BackColor = SystemColors.ActiveBorder;
                chart1.BackColor = Color.Gray;
                chart2.BackColor = Color.Gray;
                cbConnected.ForeColor = SystemColors.ControlLightLight;
                lblComPort.ForeColor = SystemColors.ControlLightLight;
                tsbDarkMode.Image = Properties.Resources.half_moon;
                pictureBox.Image = Properties.Resources.BackgroundDark;
                Properties.Settings.Default.DarkMode = true;
            }
        }
        #endregion

        #region Delegated UI elements
        private void UpdateLbl(Label lbl, string text)
        {
            Action del = () =>
            {
                lbl.Text = text;
            };
            this.Invoke(del);
        }
        private void UpdateCB(CheckBox cb, bool check)
        {
            Action del = () =>
            {
                cb.Checked = check;
            };
            this.Invoke(del);
        }
        public void UpdateConsole(string msg, Color color)
        {
            void UpdateConsoleUI()
            {
                //How many lines to keep before it removes stale readings
                while (rtbConsole.Lines.Length > 15)
                {
                    // Remove the first line
                    rtbConsole.Select(0, rtbConsole.GetFirstCharIndexFromLine(1));
                    rtbConsole.SelectedRtf = ""; // Remove the selected content preserving the formatting
                }
                rtbConsole.SelectionStart = rtbConsole.TextLength;
                rtbConsole.SelectionLength = 0;
                rtbConsole.SelectionColor = color;
                rtbConsole.AppendText(msg);
                rtbConsole.ScrollToCaret();
            }

            if (rtbConsole.InvokeRequired)
            {
                rtbConsole.Invoke((MethodInvoker)UpdateConsoleUI);
            }
            else
            {
                UpdateConsoleUI();
            }
        }
        #endregion

        #region Buttons
        private void tsbStart_Click(object sender, EventArgs e)
        {
            if (lblComPort.Text == "Devices Connected:" || lblComPort.Text == "Devices connected: 0" || lblComPort.Text == "No devices found")
            {
                tsbStart.Image = Properties.Resources.Warning;
                MessageBox.Show("No devices available to log", "No Device", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                tsbStart.ToolTipText = "Start and stop data logging";
                if (!isStarted)
                {
                    tsbConnect.Enabled = true;
                    if(cbPiSelect.SelectedItem != null)
                    {
                        selectedPort = cbPiSelect.SelectedItem.ToString();
                        ReplaceTabControlWithImage(false);
                        cbPiSelect.Enabled = false;
                        StartReading();
                        tsbConnect.ToolTipText = "Clear graph data";
                        SleepModeController.PreventSleepMode();
                    }
                    else { UpdateConsole("Please select a COM port!\n", Color.IndianRed); }
                }
                else {
                    StopReading();
                    cbPiSelect.Enabled = true;
                    SleepModeController.AllowSleepMode();
                    CloseAllPorts();
                    tsbConnect.Enabled = false;
                }
            }
        }
        private void tsbConnect_Click(object sender, EventArgs e)
        {
            if (!isStarted) { Task.Factory.StartNew(() => FindRaspberryPiPicoCOMPortAsync()); cbPiSelect.Items.Clear(); connectedPorts.Clear(); }
            else { StopReading(); ClearChart(); StartReading(); }

            if (!isRotating)
            {
                isRotating = true;
                rotationTimer.Start();
            }
        }
        private void tsbLog_Click(object sender, EventArgs e)
        {
            if (!isLogging)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                // Set the file dialog properties
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                    isLogging = true;
                    tsbLog.Image = Properties.Resources.LogGreen;
                }
            }
            else
            {
                tsbLog.Image = Properties.Resources.LogRed;
                isLogging = false;
            }
        }
        private void tsbDarkMode_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.DarkMode)
            {
                Properties.Settings.Default.DarkMode = false;
                Properties.Settings.Default.Save();
                DarkMode();
            }
            else
            {
                Properties.Settings.Default.DarkMode = true;
                Properties.Settings.Default.Save();
                DarkMode();
            }
        }
        #endregion

        #region Animations
        private void RotationTimer_Tick(object sender, EventArgs e)
        {
            RotateIcon();
        }

        private void RotateIcon()
        {
            rotationAngle += rotationSpeed * -1;
            if (rotationAngle <= -360)
            {
                rotationAngle = 0;
                rotationTimer.Stop();
                isRotating = false;
                return; // Exit early if no rotation is needed.
            }

            tsbConnect.Image?.Dispose(); // Dispose previous image if it exists.
            tsbConnect.Image = RotateImage(Properties.Resources.Refresh, rotationAngle);
        }

        private Image RotateImage(Image img, float rotationAngle)
        {
            var bmp = new Bitmap(img.Width, img.Height);

            using (var g = Graphics.FromImage(bmp))
            {
                g.TranslateTransform(img.Width / 2f, img.Height / 2f);
                g.RotateTransform(rotationAngle);
                g.TranslateTransform(-img.Width / 2f, -img.Height / 2f);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; // Example of setting quality.
                g.DrawImage(img, new Point(0, 0));
            }

            return bmp;
        }
        private void AnimateSeriesPoint(Chart chart, Series series, DateTime newX, double newY, int steps = 10, int delay = 20)
        {
            if (series.Points.Count == 0)
            {
                series.Points.AddXY(newX, newY);
                return;
            }

            int lastPermanentPointIndex = series.Points.Count - 1;
            DataPoint lastPoint = series.Points[lastPermanentPointIndex];
            DateTime startX = DateTime.FromOADate(lastPoint.XValue);
            double startY = lastPoint.YValues[0];
            TimeSpan timeSpan = newX - startX;
            double deltaY = (newY - startY) / steps;

            System.Timers.Timer timer = new System.Timers.Timer(delay);
            int currentStep = 0;
            chart.ChartAreas[0].AxisX.Minimum = DateTime.FromOADate(series.Points[0].XValue).AddSeconds(-.25).ToOADate();
            timer.Elapsed += (sender, e) =>
            {
                if (currentStep < steps)
                {
                    currentStep++;
                    DateTime intermediateX = startX.AddTicks(timeSpan.Ticks / steps * currentStep);
                    double intermediateY = startY + deltaY * currentStep;

                    if (chart.InvokeRequired)
                    {
                        chart.Invoke(new MethodInvoker(() =>
                        {
                            series.Points.AddXY(intermediateX, intermediateY);
                            chart.ChartAreas[0].AxisX.Maximum = intermediateX.AddSeconds(.5).ToOADate();
                            //AdjustYAxisScale(chart, series);
                            isAnimating = true;
                        }));
                    }
                }
                else
                {
                    timer.Stop();
                    timer.Dispose();
                    isAnimating = false;
                    if (chart.InvokeRequired)
                    {
                        chart.Invoke(new MethodInvoker(() =>
                        {
                            series.Points.AddXY(newX, newY);
                            while (series.Points.Count > lastPermanentPointIndex + 2)
                            {
                                series.Points.RemoveAt(lastPermanentPointIndex + 1);
                            }
                        }));
                    }
                }
            };
            timer.Start();
        }
        private void ReplaceTabControlWithImage(bool image)
        {
            if (image)
            {
                // Create a PictureBox
                Controls.Add(pictureBox);
                if (chart1.BackColor != Color.White) { pictureBox.Image = Properties.Resources.BackgroundDark; }
                else { pictureBox.Image = Properties.Resources.Background; }
                pictureBox.BringToFront();
                pictureBox.BorderStyle = BorderStyle.Fixed3D;
                pictureBox.Location = tcGraphs.Location;
                pictureBox.Size = tcGraphs.Size;
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            }
            else
            {
                pictureBox.Visible = false;
                tcGraphs.BringToFront();
            }
        }
        private void tableLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            ReplaceTabControlWithImage(true);
        }
        //The following is used to display the current values when the mouse is hovered over a data point on the chart
        private void CommonMouseMoveHandler(Chart chart, MouseEventArgs e)
        {
            var pos = e.Location;
            if (prevPosition.HasValue && pos == prevPosition.Value) return;
            tooltip.RemoveAll();
            prevPosition = pos;

            var results = chart.HitTest(pos.X, pos.Y, false, ChartElementType.DataPoint);

            foreach (var result in results)
            {
                if (result.ChartElementType != ChartElementType.DataPoint) continue;

                if (result.Object is DataPoint prop && result.ChartArea != null && result.Series != null)
                {
                    Axis yAxis = result.Series.YAxisType == AxisType.Secondary ? result.ChartArea.AxisY2 : result.ChartArea.AxisY;
                    var pointXPixel = result.ChartArea.AxisX.ValueToPixelPosition(prop.XValue);
                    var pointYPixel = yAxis.ValueToPixelPosition(prop.YValues[0]);

                    // Check if the cursor is within 4 pixels of the data point
                    if (Math.Abs(pos.X - pointXPixel) < 4 && Math.Abs(pos.Y - pointYPixel) < 4)
                    {
                        tooltip.Show(prop.YValues[0].ToString(), chart, pos.X, pos.Y - 15);
                        break; // Assuming only one tool tip is shown at a time
                    }
                }
            }
        }
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            CommonMouseMoveHandler(chart1, e);
        }
        private void chart2_MouseMove(object sender, MouseEventArgs e)
        {
            CommonMouseMoveHandler(chart2, e);
        }
        #endregion

        #region COM Port
        /// <summary>
        /// Looks for all connected devices with a Vendor ID matching that of the Raspberry Pi Pico/QT Pi
        /// </summary>
        public async Task FindRaspberryPiPicoCOMPortAsync()
        {
            try
            {
                // Define Raspberry Pi Pico's vendor ID (adjust if needed)
                string picoVendorId = "2E8A";  //Raspberry Pi Pico
                string qtPiVendorId = "239A";  //QT Py
                string[] portNames = SerialPort.GetPortNames();
                // Allow some time for ports to settle if needed, asynchronously
                await Task.Delay(1000);

                while (true)
                {
                    portNames = SerialPort.GetPortNames();

                    if (portNames.Length == 0)
                    {
                        // Immediately inform no ports found if array is empty
                        UpdateUIBasedOnDevicesFound(0);
                    }
                    else
                    {
                        // Use a list to collect tasks representing the port checks
                        List<Task> checkPortTasks = new List<Task>();
                        foreach (string portName in portNames)
                        {
                            if (findAllCom)
                            {
                                // If ifComAll is true, consider every COM port found
                                checkPortTasks.Add(CheckIfPortIsConnectedAsync(portName));  // Assumes a method to check if the port is connected
                            }
                            else
                            {
                                // Otherwise, check specific vendor IDs
                                checkPortTasks.Add(CheckIfVIDMatchAsync(portName, picoVendorId));
                                checkPortTasks.Add(CheckIfVIDMatchAsync(portName, qtPiVendorId));
                            }
                        }

                        // Await all the tasks to complete
                        await Task.WhenAll(checkPortTasks);

                        // Update UI based on the number of devices found
                        UpdateUIBasedOnDevicesFound(connectedPorts.Count);
                    }
                    await Task.Delay(1000);
                }
            }
            catch (Exception ex)
            {
                UpdateConsole($"[Error] Failed to detect Platform kit: {ex.Message}", Color.IndianRed);
            }
        }
        private async Task CheckIfPortIsConnectedAsync(string portName)
        {
            // This method should implement a way to determine if a port is connected and relevant
            // For simplicity, you might just simulate checking connectivity:
            await Task.Delay(10);  // Simulate some checking delay
            lock (connectedPorts)
            {
                connectedPorts.Add(portName);  // Assume connectedPorts is a list of string
            }
        }
        private async Task CheckIfVIDMatchAsync(string portName, string vendorId)
        {
            try
            {
                connectedPorts.Clear();
                string deviceId = await GetDeviceIdFromCOMPortAsync(portName);

                // Check for Pico's vendor ID and exclude Bluetooth devices, case-insensitive
                if (deviceId.ToUpper().Contains(vendorId.ToUpper()) && !deviceId.ToUpper().Contains("BTHENUM"))
                {
                    connectedPorts.Add(portName);
                }
            }
            catch (Exception ex)
            {
                // Handle and display specific port errors
                UpdateConsole($"[Error] Exception on {portName}: {ex.Message}", Color.IndianRed);
            }
        }

        private void UpdateUIBasedOnDevicesFound(int devicesFound)
        {
            if (devicesFound > 0)
            {
                Action del = () =>
                {
                    // Devices found, update UI accordingly
                    UpdateCB(cbConnected, true);
                    UpdateLbl(lblComPort, $"Devices:");
                    cbPiSelect.Enabled = true;


                    // Create a list of items to remove that are not in connectedPorts
                    var itemsToRemove = new List<string>();
                    foreach (string item in cbPiSelect.Items)
                    {
                        if (!connectedPorts.Contains(item))
                        {
                            itemsToRemove.Add(item);
                        }
                    }

                    // Remove those items from the combobox
                    foreach (string item in itemsToRemove)
                    {
                        cbPiSelect.Items.Remove(item);
                        cbPiSelect.SelectedIndex = 0;
                    }

                    // Add new items that are not already in the combobox
                    foreach (string port in connectedPorts)
                    {
                        if (!cbPiSelect.Items.Contains(port))
                        {
                            cbPiSelect.Items.Add(port);
                            cbPiSelect.SelectedIndex = 0;
                        }
                    }
                };
                this.Invoke(del);
            }
            else
            {
                Action del = () =>
                {
                    // No Pico devices found, update UI to reflect this
                    UpdateCB(cbConnected, false);
                    UpdateLbl(lblComPort, "No Device");
                    cbPiSelect.Enabled = false;
                    cbPiSelect.Items.Clear();
                    cbPiSelect.ResetText();
                };
                this.Invoke(del);
            }
        }
        private static async Task<string> GetDeviceIdFromCOMPortAsync(string portName)
        {
            return await Task.Run(() =>
            {
                string query = $"SELECT * FROM Win32_PnPEntity WHERE Caption LIKE '%(COM%'";
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject queryObj in searcher.Get())
                    {
                        string caption = queryObj["Caption"].ToString();
                        if (caption.Contains($"({portName})"))
                        {
                            return queryObj["PNPDeviceID"].ToString();
                        }
                    }
                }
                return string.Empty;
            });
        }

        /// <summary>
        /// This function should not be necessary, but is a failsafe to make sure all ports are closed on application exit
        /// </summary>
        public void CloseAllPorts()
        {
            foreach (string portName in connectedPorts)
            {
                try
                {
                    // Create a SerialPort object for the current port
                    using (SerialPort port = new SerialPort(portName))
                    {
                        // If the port is open, close it
                        if (port.IsOpen) { port.Close(); }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    // Handle exceptions (e.g., if unable to access the port)
                    UpdateConsole($"Error closing port {portName}: {ex.Message}", Color.IndianRed);
                }
            }
        }
        public void InitializePort(string portName, CancellationToken cancellationToken)
        {
            var serialPort = new SerialPort(portName) { RtsEnable = true, DtrEnable = true };

            serialPort.DataReceived += SerialPort_DataReceived;

            // Register a callback with the CancellationToken
            cancellationToken.Register(() =>
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Close(); // Close the port on cancellation
                }

                // Unsubscribe to avoid memory leaks
                serialPort.DataReceived -= SerialPort_DataReceived;


                UpdateConsole($"[{portName}] Port Closed\n", Color.SeaGreen);
            });

            try
            {
                serialPort.Open();
                UpdateConsole($"[{portName}] Port Opened\n", Color.SeaGreen);
            }
            catch (Exception ex)
            {
                UpdateConsole($"[Error] Failed to open port! {ex.Message}", Color.IndianRed);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadLine();

            // Marshal the update to the UI thread
             this.Invoke(new Action(async () =>
            {
                await UpdateChartAsync(indata);
                if (isLogging) LogToCSV(indata);
            }));
        }
        private void cbPiSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            ClearChart();
            cbPiSelect.SelectionLength = 0;
        }
        private void EvalPlatform_Resize(object sender, EventArgs e)
        {
            cbPiSelect.SelectionLength = 0;
        }
        #endregion

        #region Reading & Plotting
        public void StartReading()
        {
            cancellationTokenSource = new CancellationTokenSource();
            tsbStart.Image = Properties.Resources.Stop;
            isStarted = true;
            string com = cbPiSelect.SelectedItem.ToString();
            InitializePort(com, cancellationTokenSource.Token);
        }
        public void StopReading()
        {
            tsbStart.Image = Properties.Resources.Start;
            isStarted = false;
            cancellationTokenSource?.Cancel();
        }

        public async Task UpdateChartAsync(string input)
        {
            await chartUpdater.UpdateChartAsync(input);
        }
        private void ChartUpdater_ChartUpdated(object sender, ChartUpdaterEventArgs e)
        {
            // Ensure the UI updates are done on the UI thread
            this.Invoke((Action)(async () => // Adjusted to async lambda
            {
                // Decompose the event args
                int chartNumber = e.ChartNumber;
                string[] parts = e.Parts;
                string newTitle = parts[1];

                // Example logic from your initial UpdateChart method, adapted for use here
                if (isLogging) tsbStart.Image = Properties.Resources.Stop; // Ensure `isLogging` and `tsbStart` are accessible

                Chart targetChart = GetTargetChart(chartNumber);
                AddTabPageIfNeeded(chartNumber);
                UpdateAxisTitles();

                // Check if the title has changed
                if (lastKnownTitles.ContainsKey(chartNumber))
                {
                    if (lastKnownTitles[chartNumber] != newTitle)
                    {
                        // Title has changed, clear the chart
                        targetChart.Series.Clear();
                    }
                }
                else
                {
                    // Add entry for the first time
                    lastKnownTitles[chartNumber] = newTitle;
                }

                // Update the chart title
                UpdateChartTitle(targetChart, newTitle);

                // Convert existing series to ConcurrentDictionary
                var existingSeries = new ConcurrentDictionary<string, Series>(targetChart.Series.ToDictionary(series => series.Name, series => series));

                UpdateSeries(parts, existingSeries, targetChart);

                UpdateConsoleLog(parts);

                // Update the last known title
                lastKnownTitles[chartNumber] = newTitle;
            }));
        }

        private void UpdateChartTitle(Chart targetChart, string title)
        {
            if (targetChart.Titles.Count > 0)
            {
                targetChart.Titles[0].Text = title;
            }
            else
            {
                targetChart.Titles.Add(title);
            }
        }

        private void UpdateConsoleLog(string[] parts)
        {
            string logEntry = $"{parts[1]} {parts[0]}: ";
            UpdateConsole(logEntry, Color.Black);

            // Log the remaining parts
            for (int i = 2; i < parts.Length; i++)
            {
                UpdateConsole(parts[i], Color.Black);
            }
        }

        private Chart GetTargetChart(int chartNumber)
        {
            return chartNumber == 0 ? chart1 : chart2;
        }

        private void AddTabPageIfNeeded(int chartNumber)
        {
            if (chartNumber == 0 && !firstReadDevice1)
            {
                tcGraphs.TabPages.Add(tabPage1);
                firstReadDevice1 = true;
            }
            else if (chartNumber == 1 && !firstReadDevice2)
            {
                tcGraphs.TabPages.Add(tabPage2);
                firstReadDevice2 = true;
            }
        }

        private void UpdateAxisTitles()
        {
            SetAxisTitles(chart1);
            SetAxisTitles(chart2);
        }

        private void SetAxisTitles(Chart chart)
        {
            // Initialize variables to store axis titles
            var primaryAxisTitles = new List<string>();
            var secondaryAxisTitles = new List<string>();

            // Check and assign the YAxisType based on some condition
            for (int i = 0; i < chart.Series.Count; i++)
            {
                // Example condition: Assign every second series to the secondary axis
                if (i % 2 == 0) // Even index
                {
                    chart.Series[i].YAxisType = AxisType.Primary;
                    primaryAxisTitles.Add(chart.Series[i].Name);
                }
                else // Odd index
                {
                    chart.Series[i].YAxisType = AxisType.Secondary;
                    secondaryAxisTitles.Add(chart.Series[i].Name);
                }
            }

            // Set the title for the primary Y-axis
            chart.ChartAreas[0].AxisY.Title = string.Join(" & ", primaryAxisTitles);

            // Set the title for the secondary Y-axis
            chart.ChartAreas[0].AxisY2.Title = string.Join(" & ", secondaryAxisTitles);
        }

        private void UpdateSeries(string[] parts, ConcurrentDictionary<string, Series> existingSeries, Chart targetChart)
        {
            // Marshalling updates to the UI thread if necessary
            if (targetChart.InvokeRequired)
            {
                // Use BeginInvoke for asynchronous operation
                targetChart.BeginInvoke(new Action(() => UpdateSeries(parts, existingSeries, targetChart)));
                return;
            }

            int partsLength = parts.Length;
            for (int i = 2; i < partsLength; i += 2)
            {
                if (i + 1 >= partsLength || !double.TryParse(parts[i + 1], out double value))
                    continue; // Skip if no parsable value

                string seriesName = parts[i];
                Series series;
                bool seriesExists = false;

                // Locking or using concurrent collections for thread-safe access
                seriesExists = existingSeries.TryGetValue(seriesName, out series);
                if (!seriesExists)
                {
                    series = CreateSeries(targetChart, seriesName);
                    // Ensure this update is thread-safe
                    existingSeries[seriesName] = series; // Cache for future use
                }

                // Avoid accessing series.Points.Count repeatedly by storing in a variable
                int pointsCount = series.Points.Count;

                if (pointsCount == 1)
                {
                    AdjustAxisScale(targetChart);
                }

                series.Points.AddXY(DateTime.Now, value);
                targetChart.ChartAreas[0].AxisX.Maximum = double.NaN;
                AdjustYAxisScale(targetChart);
            }
        }

        private Series CreateSeries(Chart targetChart, string seriesName)
        {
            Series newSeries = new Series(seriesName)
            {
                //ChartType = SeriesChartType.Spline,
                ChartType = SeriesChartType.Line,
                ["LineTension"] = "0.5",
                BorderWidth = 4
            };
            targetChart.Series.Add(newSeries);
            targetChart.ChartAreas[0].AxisX.LabelStyle.Format = "hh:mm:ss";
            targetChart.ChartAreas[0].AxisY.LabelStyle.Format = "F2";
            targetChart.ChartAreas[0].AxisY2.LabelStyle.Format = "F2";
            return newSeries;
        }

        private void AdjustAxisScale(Chart chart)
        {

            foreach (ChartArea area in chart.ChartAreas)
            {
                area.AxisY.Minimum = GetMinValue(chart, 0) - 5;
                area.AxisY.Maximum = GetMaxValue(chart, 0) + 5;
                if (chart.Series.Count == 2)
                {
                    area.AxisY2.Minimum = GetMinValue(chart, 1) - 5;
                    area.AxisY2.Maximum = GetMaxValue(chart, 1) + 5;
                }
            }
        }
        private async void AdjustYAxisScale(Chart chart)
        {
            // Exit early if the chart has no series or if any series has no points
            if (chart.Series.All(s => s.Points.Count == 0)) return;

            // Create snapshots of the series and points to avoid collection modified errors
            var yAxisSnapshots = chart.Series.GroupBy(s => s.YAxisType).ToDictionary(
                g => g.Key,
                g => g.SelectMany(series => series.Points.Select(point => point.YValues[0])).ToList()
            );

            // Calculate the scale limits for each axis type on a background thread
            var axisLimits = await Task.Run(() =>
            {
                Dictionary<AxisType, (double Min, double Max)> axisMinMaxValues = new Dictionary<AxisType, (double, double)>();

                foreach (var entry in yAxisSnapshots)
                {
                    double minY = double.MaxValue;
                    double maxY = double.MinValue;

                    foreach (var yValue in entry.Value)
                    {
                        if (yValue < minY) minY = yValue;
                        if (yValue > maxY) maxY = yValue;
                    }

                    axisMinMaxValues[entry.Key] = (minY, maxY);
                }

                return axisMinMaxValues;
            });

            // Safely update the UI from the UI thread
            chart.Invoke(new MethodInvoker(() =>
            {
                foreach (var axisLimit in axisLimits)
                {
                    AxisType axisType = axisLimit.Key;
                    var (minY, maxY) = axisLimit.Value;
                    Axis axis = axisType == AxisType.Primary ? chart.ChartAreas[0].AxisY : chart.ChartAreas[0].AxisY2;

                    // Calculate and set the axis scale
                    AdjustAxisScale(axis, minY, maxY);
                }
            }));
        }

        private void AdjustAxisScale(Axis axis, double minYValue, double maxYValue)
        {
            double range = Math.Max(maxYValue - minYValue, 1); // Ensure a minimum range
            double margin = range * 0.1; // 10% margin on each end

            axis.Minimum = minYValue - margin;
            axis.Maximum = maxYValue + margin;
        }

        public static double GetMinValue(Chart chart, int seriesNumber)
        {
            if (chart.Series.Count < seriesNumber)
            {
                throw new ArgumentException("Series number is out of range");
            }

            Series series = chart.Series[seriesNumber];

            if (series.Points.Count == 0)
            {
                throw new InvalidOperationException("Series has no data points");
            }

            double minValue = series.Points.Min(point => point.YValues.Min());
            double roundedMinValue = Math.Round(minValue, 0);

            return roundedMinValue;
        }
        public static double GetMaxValue(Chart chart, int seriesNumber)
        {
            if (chart.Series.Count < seriesNumber)
            {
                throw new ArgumentException("Series number is out of range");
            }

            Series series = chart.Series[seriesNumber];

            if (series.Points.Count == 0)
            {
                throw new InvalidOperationException("Series has no data points");
            }

            double maxValue = series.Points.Max(point => point.YValues.Max());
            double roundedMaxValue = Math.Round(maxValue, 0);

            return roundedMaxValue;
        }
        
        #endregion

        #region Data Logging
        public void LogToCSV(string input)
        {
            try
            {
                // Append the input string as a new line in the CSV file
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    string[] inputSplit = input.Split();
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.Length == 0)
                    {
                        sw.Write("Date, Device #, Sensor,");
                        for (int i = 2; i < inputSplit.Length; i += 2)
                        {
                            sw.Write(inputSplit[i]);
                        }
                        sw.Write("\n");
                    }
                    // Write the input in the CSV file
                    sw.Write(DateTime.Now.ToString("M/d/yy hh:mm:ss tt") + "," + inputSplit[0] + inputSplit[1]);
                    for (int i = 3; i < inputSplit.Length; i += 2)
                    {
                        sw.Write(inputSplit[i]);
                    }
                    sw.Write("\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Logging Error]: {ex.Message}");
            }
        }
        #endregion 

        #region Chart Setup
        // Assuming you have a Chart control named chart1
        //private void SetupChart()
        //{
        //    // Enable range selection and zooming
        //    chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
        //    chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
        //    chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;

        //    chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
        //    chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
        //    chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;

        //    // Optional: Add scroll bars for convenience
        //    chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
        //    chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;

        //    // MouseWheel zooming
        //    chart1.MouseWheel += Chart1_MouseWheel;

        //    // For panning
        //    chart1.MouseDown += Chart1_MouseDown;
        //    chart1.MouseMove += Chart1_MouseMove;
        //    chart1.MouseUp += Chart1_MouseUp;
        //}

        //private Point? _previousPosition = null;
        //private System.Windows.Forms.Cursor _defaultCursor;

        //// Implementing MouseWheel event for zooming
        //private void Chart1_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    var chart = (Chart)sender;
        //    var chartArea = chart.ChartAreas[0];

        //    double xMin = chartArea.AxisX.ScaleView.ViewMinimum;
        //    double xMax = chartArea.AxisX.ScaleView.ViewMaximum;
        //    double yMin = chartArea.AxisY.ScaleView.ViewMinimum;
        //    double yMax = chartArea.AxisY.ScaleView.ViewMaximum;

        //    if (e.Delta < 0) // Zoom out
        //    {
        //        chartArea.AxisX.ScaleView.ZoomReset();
        //        chartArea.AxisY.ScaleView.ZoomReset();
        //    }
        //    else if (e.Delta > 0) // Zoom in
        //    {
        //        double posXStart = chartArea.AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
        //        double posXFinish = chartArea.AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;
        //        double posYStart = chartArea.AxisY.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 4;
        //        double posYFinish = chartArea.AxisY.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 4;
        //        chartArea.AxisX.ScaleView.Zoom(posXStart, posXFinish);
        //        chartArea.AxisY.ScaleView.Zoom(posYStart, posYFinish);
        //    }
        //}

        // Panning with mouse
        //private void Chart1_MouseDown(object sender, MouseEventArgs e)
        //{
        //    var chart = (Chart)sender;
        //    _previousPosition = e.Location;
        //    _defaultCursor = chart.Cursor;
        //    chart.Cursor = Cursors.Hand;
        //}

        //private void Chart1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    if (_previousPosition.HasValue && e.Button == MouseButtons.Left)
        //    {
        //        var chart = (Chart)sender;
        //        var pos = e.Location;
        //        var dx = pos.X - _previousPosition.Value.X;
        //        var dy = pos.Y - _previousPosition.Value.Y;

        //        var chartArea = chart.ChartAreas[0];
        //        double xConversion = (chartArea.AxisX.Maximum - chartArea.AxisX.Minimum) / chartArea.Position.Width;
        //        double yConversion = (chartArea.AxisY.Maximum - chartArea.AxisY.Minimum) / chartArea.Position.Height;

        //        chartArea.AxisX.ScaleView.Position -= dx * xConversion;
        //        chartArea.AxisY.ScaleView.Position += dy * yConversion;

        //        _previousPosition = pos;
        //    }
        //}

        //private void Chart1_MouseUp(object sender, MouseEventArgs e)
        //{
        //    var chart = (Chart)sender;
        //    _previousPosition = null;
        //    chart.Cursor = _defaultCursor;
        //}
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart1.HitTest(e.X, e.Y);

            // Adjusting the condition to check for legend items explicitly
            if (result.ChartElementType == ChartElementType.LegendItem)
            {
                // Check if the hit test result directly provides a series
                Series series = result.Series;

                // If not, try to find the series from the custom legend item
                if (series == null && result.Object is LegendItem legendItem)
                {
                    // Assuming each custom legend item has the associated series stored in its Tag
                    series = legendItem.Tag as Series;
                }

                if (series != null)
                {
                    // Toggle the enabled state of the series
                    series.Enabled = !series.Enabled;

                    // Update the legend to reflect the change
                    UpdateCustomLegendItems(series, chart1);
                }
            }
        }
        private void chart2_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = chart2.HitTest(e.X, e.Y);

            // Adjusting the condition to check for legend items explicitly
            if (result.ChartElementType == ChartElementType.LegendItem)
            {
                // Check if the hit test result directly provides a series
                Series series = result.Series;

                // If not, try to find the series from the custom legend item
                if (series == null && result.Object is LegendItem legendItem)
                {
                    // Assuming each custom legend item has the associated series stored in its Tag
                    series = legendItem.Tag as Series;
                }

                if (series != null)
                {
                    // Toggle the enabled state of the series
                    series.Enabled = !series.Enabled;

                    // Update the legend to reflect the change
                    UpdateCustomLegendItems(series, chart2);
                }
            }
        }
        private void UpdateCustomLegendItems(Series series, Chart chart)
        {
            // Remove any existing custom legend item associated with the series
            var existingItem = chart.Legends[0].CustomItems.FirstOrDefault(item => item.Tag == series);
            if (existingItem != null)
            {
                chart.Legends[0].CustomItems.Remove(existingItem);
            }

            // If series is disabled, add a new custom legend item to replace it
            if (!series.Enabled)
            {
                LegendItem customLegendItem = new LegendItem();
                customLegendItem.Name = series.Name;
                customLegendItem.ImageStyle = LegendImageStyle.Line;
                customLegendItem.Color = Color.Gray; // Gray to indicate it's disabled
                customLegendItem.Tag = series; // Store the series in the Tag for identification
                chart.Legends[0].CustomItems.Add(customLegendItem);
            }
        }
        private void ClearChart()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            chart1.ChartAreas.Clear();
            chart1.ChartAreas.Add("ChartArea1");
            chart2.Series.Clear();
            chart2.Titles.Clear();
            chart2.ChartAreas.Clear();
            chart2.ChartAreas.Add("ChartArea1");
            tcGraphs.TabPages.Clear();
            firstReadDevice1 = false;
            firstReadDevice2 = false;
        }
        #endregion

        #region Debug Mode
        private readonly List<Keys> konamiCode = new List<Keys>
        {
            Keys.Up, Keys.Up,
            Keys.Down, Keys.Down,
            Keys.Left, Keys.Right,
            Keys.Left, Keys.Right
        };
        private List<Keys> inputSequence = new List<Keys>();
        private void tsbVer_KeyDown(object sender, KeyEventArgs e)
        {
            // Add the key to the sequence.
            inputSequence.Add(e.KeyCode);

            // Ensure we only keep the last 8 inputs.
            if (inputSequence.Count > konamiCode.Count)
            {
                inputSequence.RemoveAt(0);
            }

            // Check if the input sequence matches the Konami Code.
            if (IsKonamiCodeEntered())
            {
                PerformKonamiCodeAction();
                inputSequence.Clear(); // Optional: Clear the sequence after successful detection.
            }
        }
        private bool IsKonamiCodeEntered()
        {
            if (inputSequence.Count != konamiCode.Count) return false;

            for (int i = 0; i < konamiCode.Count; i++)
            {
                if (inputSequence[i] != konamiCode[i])
                {
                    return false;
                }
            }

            return true;
        }
        private void PerformKonamiCodeAction()
        {
            MessageBox.Show("Debug mode enabled!");
            tslMem.Visible = true;
            tslCPU.Visible = true;
            tslCPUMax.Visible = true;
            tslPoints.Visible = true;

            tslMem.Text = "Memory: ";
            tslCPU.Text = "CPU: ";
            tslCPUMax.Text = "CPU Max: ";

            findAllCom = true;

            Task.Run(() => GetCurrentCpuUsage());
            Task.Run(() => UpdateMemoryUsage());
        }
        public void GetCurrentCpuUsage()
        {
            PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            double usageMax = 0;
            while (true)
            {
                using (Process process = Process.GetCurrentProcess())
                {
                    // Get the name of the process
                    string processName = Process.GetCurrentProcess().ProcessName;

                    // Create a new performance counter for the current process and CPU time
                    using (PerformanceCounter pc = new PerformanceCounter("Process", "% Processor Time", processName))
                    {
                        // First call to NextValue() always returns 0, so call it once to initialize.
                        pc.NextValue();
                        Task.Delay(1000).Wait();  // Wait a second to get a meaningful value.

                        // Get the current value, which is the CPU usage percentage
                        double usage = pc.NextValue();
                        double cpuUsagePerCore = usage / Environment.ProcessorCount;
                        if (cpuUsagePerCore > usageMax) { usageMax = cpuUsagePerCore;  }
                        
                        Invoke((MethodInvoker)delegate
                        {
                            tslCPUMax.Text = $"CPU Max: {Math.Round(usageMax, 2)} %";
                            tslCPU.Text = $"CPU: {Math.Round((double)cpuUsagePerCore, 2)} %";
                            if(Math.Round((double)cpuUsagePerCore, 2)>5)
                            {
                                tslCPU.BackColor = Color.Red;
                            }
                            else { tslCPU.BackColor = SystemColors.Control; }
                        });
                    }
                }
            }
        }

        private void UpdateMemoryUsage()
        {
            PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            while (true)
            {
                var currentProcess = Process.GetCurrentProcess();
                //var usedMemory = currentProcess.WorkingSet64 / (1024 * 1024); // Convert bytes to MB
                var usedMemory = currentProcess.PrivateMemorySize64/(1024*1024);
                Invoke((MethodInvoker)delegate {
                    tslMem.Text = $"Memory: {Math.Round((double)usedMemory, 2)} MB";
                    if (Math.Round((double)usedMemory, 2) >= 150)
                    {
                        tslMem.BackColor = Color.Red;
                    }
                    else { tslMem.BackColor = SystemColors.Control; }
                    if(!isAnimating) { tslPoints.Text = $"Total Points: {GetTotalDataPoints().ToString()}"; }
                });
                Task.Delay(1000).Wait(); // Wait for 1 second to update memory usage
            }
        }
        public int GetTotalDataPoints()
        {
            int totalDataPoints = 0;

            foreach (Series series in chart1.Series)
            {
                totalDataPoints += series.Points.Count;
            }
            foreach (Series series in chart2.Series)
            {
                totalDataPoints += series.Points.Count;
            }
            return totalDataPoints;
        }
        #endregion
    }
}

