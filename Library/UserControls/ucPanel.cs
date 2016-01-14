using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Library.UserControls
{

        public class MyPanel : Panel
        {
            public MyPanel()
            {

                this.SetStyle(ControlStyles.SupportsTransparentBackColor |
                    ControlStyles.Opaque, true);
                this.BackColor = Color.Transparent;
            }

            protected override CreateParams CreateParams
            {
                get
                {
                    CreateParams cp = base.CreateParams;
                    cp.ExStyle = 0x20;
                    return cp;
                }
            }
        }

}
