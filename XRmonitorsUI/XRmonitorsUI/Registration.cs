﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XRmonitorsUI
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();

            // https://www.eulatemplate.com/download.php?lang=en&token=37JT4i8SuiyQJj2ftgqdskJ4r5AnUfL2#
            webBrowser1.DocumentText =
@"
<h2>End-User License Agreement (EULA) of <span class=""app_name"">XRmonitors</span></h2>

<p> This End - User License Agreement(""EULA"") is a legal agreement between you and <span class=""company_name"">Augmented Perception Corporation </span></p>

<p>This EULA agreement governs your acquisition and use of our <span class=""app_name"">XRmonitors</span> software(""Software"") directly from <span class=""company_name"">Augmented Perception Corporation</span> or indirectly through a <span class=""company_name"">Augmented Perception Corporation</span> authorized reseller or distributor(a ""Reseller"").  </p>

<p>Please read this EULA agreement carefully before completing the installation process and using the <span class=""app_name"">XRmonitors</span> software.  It provides a license to use the <span class=""app_name"">XRmonitors</span> software and contains warranty information and liability disclaimers.  </p>

<p>If you register for a free trial of the <span class=""app_name"">XRmonitors</span> software, this EULA agreement will also govern that trial.  By clicking ""accept"" or installing and/or using the <span class=""app_name"">XRmonitors</span> software, you are confirming your acceptance of the Software and agreeing to become bound by the terms of this EULA agreement.  </p>

<p>If you are entering into this EULA agreement on behalf of a company or other legal entity, you represent that you have the authority to bind such entity and its affiliates to these terms and conditions.  If you do not have such authority or if you do not agree with the terms and conditions of this EULA agreement, do not install or use the Software, and you must not accept this EULA agreement.  </p>

<p>This EULA agreement shall apply only to the Software supplied by <span class=""company_name"">Augmented Perception Corporation</span> herewith regardless of whether other software is referred to or described herein.  The terms also apply to any <span class=""company_name"">Augmented Perception Corporation</span> updates, supplements, Internet-based services, and support services for the Software, unless other terms accompany those items on delivery.  If so, those terms apply.  </p>

<h3>License Grant</h3>

<p><span class=""company_name"">Augmented Perception Corporation</span> hereby grants you a personal, non-transferable, non-exclusive licence to use the<span class=""app_name"">XRmonitors</span> software on your devices in accordance with the terms of this EULA agreement.  </p>

<p>You are permitted to load the <span class=""app_name"">XRmonitors</span> software(for example a PC, laptop, mobile or tablet) under your control.  You are responsible for ensuring your device meets the minimum requirements of the <span class=""app_name"">XRmonitors</span> software.  </p>

<p>You are not permitted to:</p>

<ul>
<li>Edit, alter, modify, adapt, translate or otherwise change the whole or any part of the Software nor permit the whole or any part of the Software to be combined with or become incorporated in any other software, nor decompile, disassemble or reverse engineer the Software or attempt to do any such things.</li>
<li>Reproduce, copy, distribute, resell or otherwise use the Software for any commercial purpose.</li>
<li>Allow any third party to use the Software on behalf of or for the benefit of any third party.</li>
<li>Use the Software in any way which breaches any applicable local, national or international law.</li>
<li>Use the Software for any purpose that <span class=""company_name"">Augmented Perception Corporation</span> considers is a breach of this EULA agreement.</li>
</ul>

<h3>Intellectual Property and Ownership</h3>

<p><span class=""company_name"">Augmented Perception Corporation</span> shall at all times retain ownership of the Software as originally downloaded by you and all subsequent downloads of the Software by you.  The Software(and the copyright, and other intellectual property rights of whatever nature in the Software, including any modifications made thereto) are and shall remain the property of<span class=""company_name"">Augmented Perception Corporation</span>.  </p>

<p><span class=""company_name"">Augmented Perception Corporation</span> reserves the right to grant licences to use the Software to third parties.  </p>

<h3>Termination</h3>

<p>This EULA agreement is effective from the date you first use the Software and shall continue until terminated.  You may terminate it at any time upon written notice to<span class=""company_name"">Augmented Perception Corporation</span>.  </p>

<p>It will also terminate immediately if you fail to comply with any term of this EULA agreement.  Upon such termination, the licenses granted by this EULA agreement will immediately terminate and you agree to stop all access and use of the Software.  The provisions that by their nature continue and survive will survive any termination of this EULA agreement.  </p>

<h3>Governing Law</h3>

<p>This EULA agreement, and any dispute arising out of or in connection with this EULA agreement, shall be governed by and construed in accordance with the laws of <span class=""country"">The United States of America</span>.  </p>
";
        }

        private void FixSerialBox()
        {
            int select_start = textBox1.SelectionStart;

            String text = textBox1.Text;
            String result = "";
            foreach (char c in text.ToCharArray())
            {
                if ((c >= '0' && c <= '9') ||
                    (c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z'))
                {
                    if (result.Length == 4 ||
                        result.Length == 9 ||
                        result.Length == 12 ||
                        result.Length == 17)
                    {
                        result += "-";
                    }
                    else if (result.Length >= 22)
                    {
                        break;
                    }

                    result += char.ToUpper(c);
                }
            }

            if (text != result)
            {
                textBox1.Text = result;
                if (select_start + 1 >= text.Length)
                {
                    textBox1.SelectionStart = result.Length;
                }
                else
                {
                    textBox1.SelectionStart = select_start;
                }
            }
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                button1.Text = "Accept and Apply Key";
                FixSerialBox();
            }
            else
            {
                button1.Text = "Accept and Continue Demo";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                if (AltPcgi.SetSerialNumber(textBox1.Text.Trim()) != (int)AltPcgi.ReturnCodes.PCGI_STATUS_OK)
                {
                    MessageBox.Show("Please check the license key.");
                    return;
                }

                Console.WriteLine("Launching installer");

                AltPcgi.InvalidateSerialNumber();

                var psi = new ProcessStartInfo();
                psi.UseShellExecute = true;
                psi.FileName = Path.Combine(Program.InstallPath, "XRmonitorsSetup.exe");
                psi.Arguments = "/register " + textBox1.Text.Trim();
                Process process = Process.Start(psi);

                Console.WriteLine("Launched");

                Application.Exit();
            }

            // Software unlocked!
            Program.ShowMainForm(); this.Hide();
        }

        private void Registration_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void Registration_Shown(object sender, EventArgs e)
        {
            this.Activate();
            this.TopMost = false;
        }
    }
}