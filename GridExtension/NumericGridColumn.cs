using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace GridExtension
{


    public class NumericGridColumn : DataGridViewColumn
    {
        public NumericGridColumn() : base(new NumericGridCell()) { }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(NumericGridCell)))
                    throw new InvalidCastException("Debe ser del tipo IntegerGridCell");
                base.CellTemplate = value;
            }
        }
    }

    public class NumericGridCell : DataGridViewTextBoxCell
    {
        public NumericGridCell()
            : base()
        {
            base.MaxInputLength = 10;
        }
        
        public override Type ValueType
        {
            get { return typeof(decimal); }
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            Control ctl = DataGridView.EditingControl;
            ctl.KeyPress += new KeyPressEventHandler(IntegerGridCell_KeyPress);
        }

        private void IntegerGridCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataGridViewCell currentCell = ((IDataGridViewEditingControl)sender).EditingControlDataGridView.CurrentCell;
            if (currentCell is NumericGridCell)
            {
                if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
                {
                    e.Handled = true;
                }
                
                else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
                {
                    e.Handled = true;
                }
            }
        }

    }

    ///////////////////////////////////////////////////////////////
    public class DecimalGridColumn : DataGridViewColumn
    {
        public DecimalGridColumn() : base(new DecimalGridCell()) { }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DecimalGridCell)))
                    throw new InvalidCastException("Debe ser del tipo IntegerGridCell");
                base.CellTemplate = value;
            }
        }
    }

    public class DecimalGridCell : DataGridViewTextBoxCell
    {
        public DecimalGridCell()
            : base()
        {
            base.MaxInputLength = 10;
        }
        string separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        public override Type ValueType
        {
            get { return typeof(decimal); }
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            Control ctl = DataGridView.EditingControl;
            ctl.KeyPress += new KeyPressEventHandler(IntegerGridCell_KeyPress);
        }

        private void IntegerGridCell_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataGridViewCell currentCell = ((IDataGridViewEditingControl)sender).EditingControlDataGridView.CurrentCell;
            if (currentCell is DecimalGridCell)
            {
                if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
                {
                    e.Handled = true;
                }
                else if (e.KeyChar == ',' || e.KeyChar == '.')
                {
                    e.KeyChar = Convert.ToChar(separador);
                }
                else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
                {
                    e.Handled = true;
                }
            }
        }

    }

}
