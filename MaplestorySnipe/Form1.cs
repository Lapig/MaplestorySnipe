using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using MapleSnipe;
using System.Collections.Generic;

namespace MaplestorySnipe
{
    public partial class Form1 : Form
    {
        VAMemory vam = new VAMemory("maplestory2");
        static Process GameProcess = Process.GetProcessesByName("maplestory2").FirstOrDefault();
        LocalPlayer local = new LocalPlayer();

        public Form1()
        {
            InitializeComponent();
            Location = new Point(5, 10);
            addEventLog("Use \"Set coords\" to set the location of the purchase");
            addEventLog("button next to the first slot item on the Black Market.");
            addEventLog("If your resolution is not supported or the default");
            addEventLog("coordinates for max and second purchase are off, use");
            addEventLog("\"All manual\" to set those locations as well.");
            addEventLog("---------------------------------------------------------------------------------------------");
            runSpeedNum.Value = local.MovementSpeed();
            atkSpeedNum.Value = local.AttackSpeed();
            jumpHeightNum.Value = local.JumpHeight();
            deltaSpeedNum.Value = (decimal)local.DeltaSpeed();
            charSizeNum.Value = (decimal)local.CharSize();
            Thread tCoords = new Thread(new ThreadStart(coordsThread));
            tCoords.IsBackground = true;
            tCoords.Start();
        }

        public void addEventLog(String str) //Item sniper event log
        {
            eventLog.Items.Add(str);
            eventLog.Refresh();
            eventLog.TopIndex = eventLog.Items.Count - 1;
        }
        public void addEventLog2(String str) //Fishing bot event log
        {
            listBox1.Items.Add(str);
            listBox1.Refresh();
            listBox1.TopIndex = listBox1.Items.Count - 1;
        }
        public void addEventLog3(String str) //Debug event log
        {
            listBox2.Items.Add(str);
            listBox2.Refresh();
            listBox2.TopIndex = listBox2.Items.Count - 1;
        }

        #region Item Sniper
        public String itemName;
        public int maxBuyPrice;
        public bool runApp;
        public int activateCounter = 0;
        private void runButton_Click(object sender, EventArgs e)
        {
            if (setCoords)
            {
                runApp = true;
                maxBuyPrice = (int)maxBuyPriceBox.Value;
                addEventLog("Will buy when item is " + maxBuyPrice + " or less.");
                input.setActiveWindow("maplestory2");
                while (runApp)
                {
                    while (activateCounter < 15)
                    {
                        activateCounter++;
                        input.setActiveWindow("maplestory2");
                        clickRefresh();
                        if (getPrice() <= maxBuyPrice)
                        {
                            addEventLog("Buying for: " + getPrice());
                            clickPurchase1();
                            clickMax();
                            clickPurchase2();
                            enterYes();
                            enterOk();
                        }
                    }
                    activateMarket();
                }
                addEventLog("Lowest price was: " + min);
            }
            else addEventLog("Please use 'Set coords' before you run!");
        }

        public int min = 999999999;
        public int getPrice()
        {
            IntPtr Base = GameProcess.MainModule.BaseAddress + 0x166C734;
            IntPtr base1 = IntPtr.Add((IntPtr)vam.ReadInt32(Base), 0x20);
            IntPtr base2 = IntPtr.Add((IntPtr)vam.ReadInt32(base1), 0xC);
            IntPtr base3 = IntPtr.Add((IntPtr)vam.ReadInt32(base2), 0x40);
            if (vam.ReadInt32(base3) < min) min = vam.ReadInt32(base3);

            return vam.ReadInt32(base3);
        }

        public void activateMarket()  //Clicks the black market window
        {
            Point marketWindow = PURCHASE_BTN;
            marketWindow.X -= 250;
            Cursor.Position = marketWindow;
            input.LeftMouseClick();
            activateCounter = 0;
        }

        public void clickRefresh()
        {
            if (input.GetAsyncKeyState(123) == 0)
            {
                //Cursor.Position = REFRESH_BTN;
                //input.LeftMouseClick();
                input.CastKey(input.ScanCodes.ENTER);
                Thread.Sleep(1100);
            }
            else stop();
        }
        public void clickPurchase1()
        {
            if (input.GetAsyncKeyState(123) == 0)
            {
                Cursor.Position = PURCHASE_BTN;
                input.LeftMouseClick();
                Thread.Sleep(300);
            }
            else stop();
        }
        public void clickMax()
        {
            if (input.GetAsyncKeyState(123) == 0)
            {
                Cursor.Position = MAX_BTN;
                input.LeftMouseClick();
                Thread.Sleep(300);
            }
            else stop();
        }
        public void clickPurchase2()
        {
            if (input.GetAsyncKeyState(123) == 0)
            {
                Cursor.Position = PURCHASE2_BTN;
                input.LeftMouseClick();
                Thread.Sleep(300);
            }
            else stop();
        }
        public void enterYes()
        {
            if (input.GetAsyncKeyState(123) == 0)
            {
                //Cursor.Position = YES_BTN;
                //input.LeftMouseClick();
                input.CastKey(input.ScanCodes.ENTER);
                Thread.Sleep(300);
            }
            else stop();
        }
        public void enterOk()
        {
            if (input.GetAsyncKeyState(123) == 0)
            {
                //Cursor.Position = OK_BTN;
                //input.LeftMouseClick();
                input.CastKey(input.ScanCodes.ENTER);
                Thread.Sleep(300);
            }
            else stop();
        }

        public void stop()
        {
            runApp = false;
        }

        private void mousePos()
        {
            addEventLog("x: " + Cursor.Position.X + " y: " + Cursor.Position.Y);
        }

        Point WINDOW_SIZE = new Point();
        Point PURCHASE_BTN = new Point();
        private bool setCoords = false;

        private void setCoordsButton_Click(object sender, EventArgs e)
        {
            input.setActiveWindow("maplestory2");
            while(input.GetAsyncKeyState(113) == 0)
            {
                Thread.Sleep(100);
            }
            PURCHASE_BTN = Cursor.Position;
            addEventLog("Purchase button is at: " + "(" + PURCHASE_BTN.X + ", " + PURCHASE_BTN.Y+")");
            Thread.Sleep(750);
            if (allManual.Checked)
            {
                while (input.GetAsyncKeyState(113) == 0)
                {
                    Thread.Sleep(100);
                }
                MAX_BTN = Cursor.Position;
                addEventLog("Max button is at: " + "(" + MAX_BTN.X + ", " + MAX_BTN.Y + ")");
                Thread.Sleep(750);
                while (input.GetAsyncKeyState(113) == 0)
                {
                    Thread.Sleep(100);
                }
                PURCHASE2_BTN = Cursor.Position;
                addEventLog("Second purchase button is at: " + "(" + PURCHASE2_BTN.X + ", " + PURCHASE2_BTN.Y + ")");
                Thread.Sleep(750);
                Activate();
                setCoords = true;
            }
            else
            {
                setResPositions();
                Activate();
                setCoords = true;
            }
        }
        #endregion

        #region Various Resolution Settings
        Point PLAY_MINI = new Point();
        Point MAX_BTN = new Point();
        Point PURCHASE2_BTN = new Point();
        private void setResPositions()
        {
            Point PLAY_MINI_1920x1080 = new Point(875 + getOffset().X, 653 + getOffset().Y);
            Point MAX_BTN_1920x1080 = new Point(1055 + getOffset().X, 625 + getOffset().Y);
            Point PURCHASE2_BTN_1920x1080 = new Point(911 + getOffset().X, 675 + getOffset().Y);

            Point PLAY_MINI_1280x768 = new Point(584 + getOffset().X, 463 + getOffset().Y);
            Point MAX_BTN_1280x768 = new Point(707 + getOffset().X, 441 + getOffset().Y);
            Point PURCHASE2_BTN_1280x768 = new Point(605 + getOffset().X, 477 + getOffset().Y);
            switch (getResolution())
            {
                case "1920x1080":
                    PLAY_MINI = PLAY_MINI_1920x1080;
                    MAX_BTN = MAX_BTN_1920x1080;
                    PURCHASE2_BTN = PURCHASE2_BTN_1920x1080;
                    break;
                case "1280x768":
                    PLAY_MINI = PLAY_MINI_1280x768;
                    MAX_BTN = MAX_BTN_1280x768;
                    PURCHASE2_BTN = PURCHASE2_BTN_1280x768;
                    break;
            }
        }

        private string getResolution()
        {
            int x = input.getWindowSize("maplestory2").Width;
            int y = input.getWindowSize("maplestory2").Height;
            if (1920 <= x && x <= 1930 && 1080 <= y && y <= 1110) return "1920x1080";
            else if (1280 <= x && x <= 1290 && 768 <= y && y <= 798) return "1280x768";
            else if (1768 <= x && x <= 1778 && 992 <= y && y <= 1022) return "1768x992";
            else return "null";
        }
        #endregion

        #region Fishing Bot
        public string fishingState()
        {
            IntPtr Base = GameProcess.MainModule.BaseAddress + 0x166A118;
            IntPtr base1 = IntPtr.Add((IntPtr)vam.ReadInt32(Base), 0x20);
            IntPtr base2 = IntPtr.Add((IntPtr)vam.ReadInt32(base1), 0x84);
            IntPtr base3 = IntPtr.Add((IntPtr)vam.ReadInt32(base2), 0x20);
            IntPtr base4 = IntPtr.Add((IntPtr)vam.ReadInt32(base3), 0x8);
            IntPtr base5 = IntPtr.Add((IntPtr)vam.ReadInt32(base4), 0x88);
            int fishingValue = vam.ReadInt32(base5);
            if (fishingValue > 5000) return "fishing";
            else if (fishingValue == 5000) return "mini-game";
            else return "not fishing";
        }

        public void playMini()
        {
            Thread.Sleep(1100);
            while (fishingState() == "mini-game")
            {
                /*for (int i = -10; i < 20; i++)                                //Old code that read pixels instead of just timing it
                {
                    int col = input.GetColorAt(PLAY_MINI.X, PLAY_MINI.Y+i);
                    if (10003000 < col && col < 18865301)
                    {
                        Thread.Sleep(135);
                        input.CastKey(getActionKey());
                        Thread.Sleep(163);
                    }                
                    else
                    {
                        Thread.Sleep(3);
                    }
                }*/
                input.CastKey(getActionKey());
                Thread.Sleep(310);
            }
        }

        private void fishingStartButton_Click(object sender, EventArgs e)
        {
            if (getActionKey() != 0)
            {
                input.setActiveWindow("maplestory2");
                addEventLog2("Starting fishing bot");
                setResPositions();
                Thread.Sleep(500);
                while (input.GetAsyncKeyState(123) == 0)
                {
                    switch (fishingState())
                    {
                        case "not fishing":
                            input.CastKey(getActionKey());
                            Thread.Sleep(500);
                            break;
                        case "fishing":
                            Thread.Sleep(50);
                            break;
                        case "mini-game":
                            addEventLog2("Mini-game detected");
                            playMini();
                            break;
                    }
                }
                addEventLog2("Stopping");
            }
            else addEventLog2("Please select your action key.");
        }

        private short getActionKey()
        {
            return getScanCode(comboBox1.Text);
        }
        #endregion

        #region Debug Tab
        private void button3_Click_1(object sender, EventArgs e)
        {
            WINDOW_SIZE.X = input.getWindowSize("maplestory2").Width - 6;
            WINDOW_SIZE.Y = input.getWindowSize("maplestory2").Height - 29;
            while (input.GetAsyncKeyState(114) == 0)
            {
                Thread.Sleep(1000);
                addEventLog3("X: " + MousePosition.X + " Y: " + MousePosition.Y + "   Color: " + input.GetColorAt(MousePosition.X, MousePosition.Y) + "");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.gamekiller.net/forums/maplestory-2-global-europe-hacks-cheats-bots.880/");
        }

        private static Point getOffset()
        {
            try
            {
                Point offset = input.getWindowPos("maplestory2");
                offset.X += 2;
                offset.Y += 24;
                return offset;
            } catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Please start MapleStory 2 first.");
                Environment.Exit(1);
                return MousePosition;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            WINDOW_SIZE.X = input.getWindowSize("maplestory2").Width - 6;
            WINDOW_SIZE.Y = input.getWindowSize("maplestory2").Height - 29;
            int xOffset = input.getWindowPos("maplestory2").X + 2;
            int yOffset = input.getWindowPos("maplestory2").Y + 24;
            while (input.GetAsyncKeyState(114) == 0)
            {
                Thread.Sleep(500);
                addEventLog3(local.JumpHeight()+"");
                addEventLog3("Window size: " + input.getWindowSize("maplestory2").Width + "x" + input.getWindowSize("maplestory2").Height);
                addEventLog3(getResolution());
                addEventLog3("X: " + (MousePosition.X-xOffset) + " Y: " + (MousePosition.Y-yOffset) + "  Color: " + input.GetColorAt((MousePosition.X - xOffset), (MousePosition.Y - yOffset)));
            }
        }
        #endregion

        #region AFK Tool Tab
        private bool hold1 = false;
        private bool hold2 = false;
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Enabled == true) numericUpDown1.Enabled = false;
            else numericUpDown1.Enabled = true;
            if (hold1 == false) hold1 = true;
            else hold1 = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Enabled == true) numericUpDown2.Enabled = false;
            else numericUpDown2.Enabled = true;
            if (hold2 == false) hold2 = true;
            else hold2 = false;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                comboBox2.Enabled = false;
                checkBox1.Enabled = false;
                numericUpDown1.Enabled = false;
            }
            else
            {
                comboBox2.Enabled = true;
                checkBox1.Enabled = true;
                if (!hold1) numericUpDown1.Enabled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                comboBox3.Enabled = false;
                checkBox2.Enabled = false;
                numericUpDown2.Enabled = false;
            }
            else
            {
                comboBox3.Enabled = true;
                checkBox2.Enabled = true;
                if (!hold2) numericUpDown2.Enabled = true;
            }
        }

        private short key1;
        private short key2;
        private int wait1;
        private int wait2;
        private void button4_Click(object sender, EventArgs e)
        {
            input.setActiveWindow("maplestory2");
            runAFK = true;
            key1 = getScanCode(comboBox2.Text);
            key2 = getScanCode(comboBox3.Text);
            wait1 = (int)numericUpDown1.Value;
            wait2 = (int)numericUpDown2.Value;
            Thread tid1 = new Thread(new ThreadStart(Thread1));
            tid1.IsBackground = true;
            Thread tid2 = new Thread(new ThreadStart(Thread2));
            tid2.IsBackground = true;
            tid1.Start();
            tid2.Start();
            if (hold1 || hold2)
            {
                input.CastKeyUp(getScanCode(comboBox2.Text));
                input.CastKeyUp(getScanCode(comboBox3.Text));
            }
        }

        private bool runAFK = false;
        private void Thread1()
        {
            while (runAFK)
            {
                if (!hold1 && !checkBox3.Checked)
                {
                    input.CastKey(key1);
                    Thread.Sleep(wait1);
                    if (input.GetAsyncKeyState(123) != 0) runAFK = false;
                }
                else if (hold1 && !checkBox3.Checked)
                {
                    input.CastKeyDown(key1);
                    Thread.Sleep(50);
                    if (input.GetAsyncKeyState(123) != 0)
                    {
                        input.CastKeyUp(key1);
                        runAFK = false;
                    }
                }
            }
        }
        private void Thread2()
        {
            while (runAFK)
            {
                if (!hold2 && !checkBox4.Checked)
                {
                    input.CastKey(key2);
                    Thread.Sleep(wait2);
                    if (input.GetAsyncKeyState(123) != 0) runAFK = false;
                }
                else if (hold2 && !checkBox4.Checked)
                {
                    input.CastKeyDown(key2);
                    Thread.Sleep(50);
                    if (input.GetAsyncKeyState(123) != 0)
                    {
                        input.CastKeyUp(key2);
                        runAFK = false;
                    }
                }
            }
        }
        #endregion

        #region Value Editor
        private void runSpeedButton_Click(object sender, EventArgs e)
        {
            stopRun = false;
            Thread runSpeed = new Thread(new ThreadStart(runSpeedThread));
            runSpeed.IsBackground = true;
            runSpeed.Start();
        }
        private void runSpeedThread()
        {
            while (!stopRun)
            {
                int setValue = (int)runSpeedNum.Value;
                while (local.MovementSpeed() != setValue)
                {
                    local.setMovementSpeed(setValue);
                }
                Thread.Sleep(500);
            }
        }

        private void atkSpeedButton_Click(object sender, EventArgs e)
        {
            stopAtk = false;
            Thread atkSpeed = new Thread(new ThreadStart(atkSpeedThread));
            atkSpeed.IsBackground = true;
            atkSpeed.Start();
        }
        private void atkSpeedThread()
        {
            while (!stopAtk)
            {
                int setValue = (int)atkSpeedNum.Value;
                while (local.AttackSpeed() != setValue)
                {
                    local.setAttackSpeed(setValue);
                }
                Thread.Sleep(10);
            }
        }

        private void deltaSpeedStart_Click(object sender, EventArgs e)
        {
            stopDelta = false;
            Thread deltaSpeed = new Thread(new ThreadStart(deltaSpeedThread));
            deltaSpeed.IsBackground = true;
            deltaSpeed.Start();
        }
        private void deltaSpeedThread()
        {
            while (!stopDelta)
            {
                float setValue = (float)deltaSpeedNum.Value;
                while (local.DeltaSpeed() != setValue)
                {
                    local.setDeltaSpeed((float)setValue);
                }
                Thread.Sleep(10);
            }
        }

        private void jumpHeightButton_Click(object sender, EventArgs e)
        {
            stopJump = false;
            Thread jumpHeight = new Thread(new ThreadStart(jumpHeightThread));
            jumpHeight.IsBackground = true;
            jumpHeight.Start();
        }
        private void jumpHeightThread()
        {
            while (!stopJump)
            {
                int setValue = (int)jumpHeightNum.Value;
                while (local.JumpHeight() != setValue)
                {
                    local.setJumpHeight(setValue);
                }
                Thread.Sleep(10);
            }
        }

        private void charSizeSet_Click(object sender, EventArgs e)
        {
            Thread charSize = new Thread(new ThreadStart(charSizeThread));
            charSize.IsBackground = true;
            charSize.Start();
        }
        private void charSizeThread()
        {
            float setValue = (float)charSizeNum.Value;
            while (local.CharSize() != setValue)
            {
                local.setCharSize(setValue);
            }
            Thread.Sleep(10);
        }

        private bool stopDelta;
        private void deltaSpeedStop_Click(object sender, EventArgs e)
        {
            stopDelta = true;
        }
        private bool stopRun;
        private void runSpeedStop_Click(object sender, EventArgs e)
        {
            stopRun = true;
        }
        private bool stopAtk;
        private void atkStop_Click_1(object sender, EventArgs e)
        {
            stopAtk = true;
        }
        private bool stopJump;
        private void button1_Click_3(object sender, EventArgs e)
        {
            stopJump = true;
        }
        #endregion

        #region Teleportation
        delegate void StringArgReturningVoidDelegate(string text, string text2, string text3);
        private void setText(string text, string text2, string text3)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.xCoordLabel.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(setText);
                this.Invoke(d, new object[] { text, text2, text3 });
            }
            else
            {
                this.xCoordLabel.Text = text;
                this.yCoordsLabel.Text = text2;
                this.zCoordsLabel.Text = text3;
            }
        }

        private void coordsThread()
        {
            while (true)
            {
                try
                {
                    setText(local.Coords().X_AXIS + "", local.Coords().Y_AXIS + "", local.Coords().Z_AXIS + "");
                    Thread.Sleep(50);
                }
                catch (ObjectDisposedException) { Environment.Exit(0); };
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            xCoordNum.Value = (decimal)float.Parse(xCoordLabel.Text);
            yCoordNum.Value = (decimal)float.Parse(yCoordsLabel.Text);
            zCoordNum.Value = (decimal)float.Parse(zCoordsLabel.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            local.teleport((float)xCoordNum.Value, (float)yCoordNum.Value, (float)zCoordNum.Value);
            input.setActiveWindow("maplestory2");
            if (xCoordLockCheck.Checked)
            {
                Thread xLock = new Thread(new ThreadStart(xCoordLocked));
                xLock.Start();
            }
            if (yCoordLockCheck.Checked)
            {
                Thread yLock = new Thread(new ThreadStart(yCoordLocked));
                yLock.Start();
            }
            if (zCoordLockCheck.Checked)
            {
                Thread zLock = new Thread(new ThreadStart(zCoordLocked));
                zLock.Start();
            }
        }

        bool xLocked = false;
        private void xCoordLocked()
        {
            float value = (float)xCoordNum.Value;
            while (xLocked)
            {
                local.writeValueFloat((local.getAddressLevelFour(local.localPlayerBase, LocalPlayer.OffSets.X_COORD_1, LocalPlayer.OffSets.X_COORD_2, LocalPlayer.OffSets.X_COORD_3, LocalPlayer.OffSets.X_COORD_4)), value);
                Thread.Sleep(1);
            }
        }
        private void xCoordLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (xCoordLockCheck.Checked) xLocked = true;
            else xLocked = false;
        }

        bool yLocked = false;
        private void yCoordLocked()
        {
            float value = (float)yCoordNum.Value;
            while (yLocked)
            {
                local.writeValueFloat((local.getAddressLevelFour(local.localPlayerBase, LocalPlayer.OffSets.Y_COORD_1, LocalPlayer.OffSets.Y_COORD_2, LocalPlayer.OffSets.Y_COORD_3, LocalPlayer.OffSets.Y_COORD_4)), value);
                Thread.Sleep(1);
            }
        }
        private void yCoordLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (yCoordLockCheck.Checked) yLocked = true;
            else yLocked = false;
        }

        bool zLocked = false;
        private void zCoordLocked()
        {
            float value = (float)zCoordNum.Value;
            while (zLocked)
            {
                local.writeValueFloat((local.getAddressLevelFour(local.localPlayerBase, LocalPlayer.OffSets.Z_COORD_1, LocalPlayer.OffSets.Z_COORD_2, LocalPlayer.OffSets.Z_COORD_3, LocalPlayer.OffSets.Z_COORD_4)), value);
                Thread.Sleep(1);
            }
        }
        private void zCoordLockCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (zCoordLockCheck.Checked) zLocked = true;
            else zLocked = false;
        }
        #endregion

        private static short getScanCode(string value)
        {
            switch (value)
            {
                case "ONE": return input.ScanCodes.ONE;
                case "TWO": return input.ScanCodes.TWO;
                case "THREE": return input.ScanCodes.THREE;
                case "FOUR": return input.ScanCodes.FOUR;
                case "FIVE": return input.ScanCodes.FIVE;
                case "SIX": return input.ScanCodes.SIX;
                case "SEVEN": return input.ScanCodes.SEVEN;
                case "EIGHT": return input.ScanCodes.EIGHT;
                case "NINE": return input.ScanCodes.NINE;
                case "A": return input.ScanCodes.A;
                case "B": return input.ScanCodes.B;
                case "C": return input.ScanCodes.C;
                case "D": return input.ScanCodes.D;
                case "E": return input.ScanCodes.E;
                case "F": return input.ScanCodes.F;
                case "G": return input.ScanCodes.G;
                case "H": return input.ScanCodes.H;
                case "I": return input.ScanCodes.I;
                case "J": return input.ScanCodes.J;
                case "K": return input.ScanCodes.K;
                case "L": return input.ScanCodes.L;
                case "M": return input.ScanCodes.M;
                case "N": return input.ScanCodes.N;
                case "O": return input.ScanCodes.O;
                case "P": return input.ScanCodes.P;
                case "Q": return input.ScanCodes.Q;
                case "R": return input.ScanCodes.R;
                case "S": return input.ScanCodes.S;
                case "T": return input.ScanCodes.T;
                case "U": return input.ScanCodes.U;
                case "V": return input.ScanCodes.V;
                case "W": return input.ScanCodes.W;
                case "X": return input.ScanCodes.X;
                case "Y": return input.ScanCodes.Y;
                case "Z": return input.ScanCodes.Z;
                case "SPACE": return input.ScanCodes.SPACE;
                case "UP": return input.ScanCodes.UP;
                case "DOWN": return input.ScanCodes.DOWN;
                case "LEFT": return input.ScanCodes.LEFT;
                case "RIGHT": return input.ScanCodes.RIGHT;
            }
            return 0;
        }

    }
}
