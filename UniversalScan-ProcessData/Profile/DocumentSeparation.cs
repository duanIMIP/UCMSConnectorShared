using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IMIP.UniversalScan.Profile
{
    public partial class DocumentSeparation : UserControl
    {
        #region Events
        public delegate void SettingChangedEvent();
        public event SettingChangedEvent SettingChanged;
        #endregion Events

        #region Member Fields
        private const string Barcode128 = "Code 128";
        private const string Barcode39 = "Code 39";
        private const string BarcodeIndustrial2of5 = "Industrial 2 of 5";
        //private const string BarcodeInverted2of5 = "Inverted 2 of 5";
        private const string BarcodeInterleaved2of5 = "Interleaved 2 of 5";
        private const string BarcodeIata2of5 = "Iata 2 of 5";
        private const string BarcodeMatrix2of5 = "Matrix 2 of 5";
        private const string BarcodeCodeabar = "Codabar";
        //private const string BarcodedMatrix = "Matrix";
        private const string BarcodeDataLogic2of5 = "DataLogic 2 of 5";
        private const string Barcode93 = "Code 93";
        private const string BarcodeEAN13 = "EAN 13";
        private const string BarcodeUPCA = "UPC A";
        private const string BarcodeEAN8 = "EAN 8";
        private const string BarcodeUPCE = "UPC E";
        private const string BarcodeADD5 = "ADD 5";
        private const string BarcodeADD2 = "ADD 2";

        private const string BarcodeQR = "QR Code";
        //private const string BarcodeDataMatrix = "Data Matrix";
        //private const string BarcodePDF417 = "PDF 417";

        private const string OperatorStartWith = "Start With";
        private const string OperatorContain = "Contain";
        private const string OperatorEndWith = "End With";
        private const string OperatorExact = "Exact";

        private const string MessageValidation1 = "In order to proceed, you can either complete or delete current setting.";
        private const string MessageAskDeleteRow = "Are you sure to delete current setting?";

        SeparationProfile m_SeparationProfile;

        private Collection<string> m_FormTypes;
        private bool m_bLoading = false;
        private bool m_bBarcodeSeparationSettingChanged = false;
        #endregion Member Fields

        #region Constructor
        public DocumentSeparation()
        {
            InitializeComponent();

            m_SeparationProfile = new UniversalScan.Profile.SeparationProfile();
            m_FormTypes = new Collection<string>();

            dgvBarcode.Dock = DockStyle.Fill;
        }
        #endregion Constructor

        #region Properties
        public SeparationProfile SeparationProfile
        {
            get 
            {
                IMIP.UniversalScan.Profile.SeparationProfile separationProfile = new IMIP.UniversalScan.Profile.SeparationProfile();

                separationProfile.Active = chkEnabledSeparation.Checked;

                //separation type
                if (radFixPage.Checked)
                {
                    separationProfile.eSeparationType = SeparationType.FixPage;
                    //fix page separation
                    if (cboFPFormType.SelectedItem != null)
                    {
                        separationProfile.FixPageSeparationSetting.FormType = cboFPFormType.SelectedItem.ToString(); //cboFPFormType.Text;
                        separationProfile.FixPageSeparationSetting.PageCount = (int)nudPageCount.Value;
                    }
                }
                else if (radBlankPage.Checked)
                {
                    separationProfile.eSeparationType = SeparationType.BlankPage;
                    //blank page separation
                    if (cboBPFormType.SelectedItem != null)
                    {
                        separationProfile.BlankPageSeparationSetting.FormType = cboBPFormType.SelectedItem.ToString(); //cboBPFormType.Text;
                        separationProfile.BlankPageSeparationSetting.Threshold = (int)nudThreshold.Value;
                        separationProfile.BlankPageSeparationSetting.DiregardPage = chkBPDeleteBlankPage.Checked;
                    }
                }
                else if (radBarcode.Checked)
                {
                    separationProfile.eSeparationType = SeparationType.Barcode;
                    //barcode separation
                    if (dgvBarcode.Rows.Count > 1)
                    {
                        foreach (DataGridViewRow oRow in dgvBarcode.Rows)
                        {
                            if (!oRow.IsNewRow)
                                separationProfile.BarcodeSeparationSettings.Add(PopulateBarcodeSeparationSetting(oRow));
                        }
                    }
                }

                else if (radCustom.Checked)
                {
                    separationProfile.eSeparationType = SeparationType.Custom;
                    separationProfile.CustomSeparationSetting.ProjectFile = textBoxCustomFile.Text;
                }

                return separationProfile; 
            }
        }
        #endregion Properties

        #region Public Methods
        public void Init(Collection<string> formTypes, SeparationProfile separationProfile)
        {
            m_bLoading = true;

            chkEnabledSeparation.Checked = false;
            grbSeparationType.Enabled = false;
            grbSepDetail.Enabled = false;
            m_FormTypes = formTypes;
            //m_SeparationProfile = separationProfile;
            InitValue(separationProfile);

            if (formTypes == null)
                return;

            try
            {
                FillDocumentTypeCombo(cboFPFormType);
                FillDocumentTypeCombo(cboBPFormType);
                dgvBarcode.Rows.Clear();
                FillDocumentTypeGridViewCombo(colFormType);
                FillBarcodeType();
                FillComparison();

                if (separationProfile != null)
                    FillSettingsSeparationProfile(separationProfile);
            }
            finally
            {
                m_bLoading = false;
            }
        }

        public string ValidateSettings()
        {
            string sResult = "";

            if (chkEnabledSeparation.Checked)
            {
                if (radFixPage.Checked)
                {
                    sResult = ValidateFixPageSetting();
                }
                else if (radBlankPage.Checked)
                {
                    sResult = ValidateBlankPageSetting();
                }
                else if (radBarcode.Checked)
                {
                    if (dgvBarcode.RowCount <= 1)
                        return "No barcode separation setting was specified.";

                    foreach (DataGridViewRow oRow in dgvBarcode.Rows)
                    {
                        if (!oRow.IsNewRow)
                        {
                            sResult = ValidateBarcodeSeparationSetting(oRow);
                            if (sResult != "")
                                break;
                        }
                    }
                }
                else //radCustom.Checked = true
                {
                    if (textBoxCustomFile.Text == "")
                        return "No profile has been specified.";
                    //else
                    //{
                    //    if (!System.IO.File.Exists(this.textBoxCustomFile.Text))
                    //        return "Profile doesnt exists.";
                    //}
                }
            }

            return sResult;
        }

        public void RemoveFormType(string sFormTypeName)
        {
            m_FormTypes.Remove(sFormTypeName);
            cboFPFormType.Items.Remove(sFormTypeName);
            cboBPFormType.Items.Remove(sFormTypeName);

            //remove barcode separation setting(s) related to given form type name
            int nCount = dgvBarcode.Rows.Count;
            for (int nIndex = nCount - 1; nIndex >= 0; nIndex--)
            {
                DataGridViewRow row = dgvBarcode.Rows[nIndex];
                if (sFormTypeName.Equals(row.Cells[0].FormattedValue.ToString(), StringComparison.OrdinalIgnoreCase))
                    dgvBarcode.Rows.RemoveAt(nIndex);
            }
            colFormType.Items.Remove(sFormTypeName);
        }

        public void AddFormType(string sFormTypeName)
        {
            m_FormTypes.Add(sFormTypeName);
            cboFPFormType.Items.Add(sFormTypeName);
            cboBPFormType.Items.Add(sFormTypeName);
            colFormType.Items.Add(sFormTypeName);
        }

        public void RenameFormType(string sOldValue, string sNewValue)
        {
            RenameComboItem(cboFPFormType, sOldValue, sNewValue);
            RenameComboItem(cboBPFormType, sOldValue, sNewValue);
            UpdateCellValue(0, sOldValue, sNewValue);
            UpdateDataGridViewComboItem(colFormType, sOldValue, sNewValue);
        }
        #endregion Public Methods

        #region Private Methods
        private void FillDocumentTypeCombo(ComboBox comboBox)
        {
            comboBox.Items.Clear();
            foreach (string docTypeName in m_FormTypes)
            {
                comboBox.Items.Add(docTypeName);
            }
        }

        private void FillDocumentTypeGridViewCombo(DataGridViewComboBoxColumn comboBox)
        {
            comboBox.Items.Clear();
            foreach (string docTypeName in m_FormTypes)
            {
                comboBox.Items.Add(docTypeName);
            }
        }

        private void FillBarcodeType()
        {
            colBarcode.Items.Clear();
            colBarcode.Items.Add(Barcode128);
            colBarcode.Items.Add(Barcode39);
            colBarcode.Items.Add(Barcode93);
            
            colBarcode.Items.Add(BarcodeIata2of5);
            colBarcode.Items.Add(BarcodeMatrix2of5);
            //colBarcode.Items.Add(BarcodeInverted2of5);
            colBarcode.Items.Add(BarcodeDataLogic2of5);
            colBarcode.Items.Add(BarcodeIndustrial2of5);
            colBarcode.Items.Add(BarcodeInterleaved2of5);

            colBarcode.Items.Add(BarcodeCodeabar);
            
            //colBarcode.Items.Add(BarcodedMatrix);
            colBarcode.Items.Add(BarcodeEAN8);
            colBarcode.Items.Add(BarcodeEAN13);

            colBarcode.Items.Add(BarcodeUPCA);
            colBarcode.Items.Add(BarcodeUPCE);

            colBarcode.Items.Add(BarcodeADD2);
            colBarcode.Items.Add(BarcodeADD5);

            colBarcode.Items.Add(BarcodeQR);
        }

        private void FillComparison()
        {
            colComparision.Items.Clear();
            colComparision.Items.Add(OperatorStartWith);
            colComparision.Items.Add(OperatorContain);
            colComparision.Items.Add(OperatorEndWith);
            colComparision.Items.Add(OperatorExact);
        }

        private void FillSettingsSeparationProfile(SeparationProfile separationProfile)
        {
            if (separationProfile == null)
            {
                chkEnabledSeparation.Checked = false;
                grbSeparationType.Enabled = false;
                grbSepDetail.Enabled = false;
                return;
            }

            chkEnabledSeparation.Checked = separationProfile.Active;
            grbSeparationType.Enabled = chkEnabledSeparation.Checked;
            grbSepDetail.Enabled = chkEnabledSeparation.Checked;

            switch (separationProfile.eSeparationType)
            {
                case SeparationType.FixPage:
                    radFixPage.Checked = true;
                    tabSeparation.SelectedTab = tabPageFixPage;
                    panelFixPage.Enabled = true;
                    panelBlankPage.Enabled = false;
                    panelBarcode.Enabled = false;
                    panelCustom.Enabled = false;
                    //FillSettingsSeparationFixPage(separationProfile.FixPageSeparationSetting);
                    break;
                case SeparationType.BlankPage:
                    radBlankPage.Checked = true;
                    tabSeparation.SelectedTab = tabPageBlankPage;
                    panelFixPage.Enabled = false;
                    panelBlankPage.Enabled = true;
                    panelBarcode.Enabled = false;
                    panelCustom.Enabled = false;
                    //FillSettingsSeparationBlankPage(separationProfile.BlankPageSeparationSetting);
                    break;
                case SeparationType.Barcode:
                    radBarcode.Checked = true;
                    tabSeparation.SelectedTab = tabPageBarcode;
                    panelFixPage.Enabled = false;
                    panelBlankPage.Enabled = false;
                    panelBarcode.Enabled = true;
                    panelCustom.Enabled = false;
                    //FillSettingsSeparationBarcode(separationProfile);
                    break;

                case SeparationType.Custom:
                    radCustom.Checked = true;
                    tabSeparation.SelectedTab = tabPageCustom;
                    panelFixPage.Enabled = false;
                    panelBlankPage.Enabled = false;
                    panelBarcode.Enabled = false;
                    panelCustom.Enabled = true;
                    //FillSettingsSeparationBarcode(separationProfile);
                    break;
            }

            FillSettingsSeparationFixPage(separationProfile.FixPageSeparationSetting);
            FillSettingsSeparationBlankPage(separationProfile.BlankPageSeparationSetting);
            FillSettingsSeparationBarcode(separationProfile);
            textBoxCustomFile.Text = separationProfile.CustomSeparationSetting.ProjectFile;
        }

        private void FillSettingsSeparationFixPage(FixPageSeparationSetting oFPSetting)
        {
            nudPageCount.Value = oFPSetting.PageCount;

            if (string.IsNullOrEmpty(oFPSetting.FormType))
            {
                if (cboFPFormType.Items.Count > 0)
                    cboFPFormType.SelectedIndex = 0;
                return;
            }

            if (IsFormTypeExistence(oFPSetting.FormType))
            {
                cboFPFormType.SelectedItem = oFPSetting.FormType;
            }
            else
            {
                string sMsg = string.Format("Document type '{0}' of current fix page separation setting does not exist.", oFPSetting.FormType);
                MessageBox.Show(this, sMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FillSettingsSeparationBlankPage(BlankPageSeparationSetting oBPSetting)
        {
            nudThreshold.Value = (decimal)oBPSetting.Threshold;
            chkBPDeleteBlankPage.Checked = oBPSetting.DiregardPage;

            if (string.IsNullOrEmpty(oBPSetting.FormType))
            {
                if (cboBPFormType.Items.Count > 0)
                    cboBPFormType.SelectedIndex = 0;
                return;
            }

            if (IsFormTypeExistence(oBPSetting.FormType))
            {
                cboBPFormType.SelectedItem = oBPSetting.FormType;
            }
            else
            {
                string sMsg = string.Format("Document type '{0}' of current blank page separation setting does not exist.", oBPSetting.FormType);
                MessageBox.Show(this, sMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FillSettingsSeparationBarcode(SeparationProfile separationProfile)
        {
            dgvBarcode.Rows.Clear();

            Collection<BarcodeSeparationSetting> oBarcodeSeparationSettings = new Collection<BarcodeSeparationSetting>();
            int nCounter = 0;
            foreach (BarcodeSeparationSetting oSetting in separationProfile.BarcodeSeparationSettings)
            {
                string formType = oSetting.FormType;
                if (IsFormTypeExistence(formType))
                {
                    string barcode = GetBarcodeValue(oSetting.eBarcode);
                    string sOperator = GetOperator(oSetting.eComparison);
                    string compareValue = oSetting.CompareValue;
                    string deletePage = oSetting.DiregardPage.ToString();
                    string combineMinPage = oSetting.CombineCheckWithMinimumPageCount.ToString();
                    string minPage = oSetting.MinimumPageCount.ToString();
                    dgvBarcode.Rows.Add(new string[] { formType, barcode, sOperator, compareValue, deletePage, combineMinPage, minPage });
                    dgvBarcode.Rows[nCounter].Cells[6].ReadOnly = !oSetting.CombineCheckWithMinimumPageCount;
                    nCounter++;
                    oBarcodeSeparationSettings.Add(oSetting);
                }
                else
                {
                    string sMsg = string.Format("Document type '{0}' does not exist. Barcode separation setting tied with this document type will be deleted.", formType);
                    MessageBox.Show(this, sMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            //remove settings whose form type no longer exist
            if (oBarcodeSeparationSettings.Count < separationProfile.BarcodeSeparationSettings.Count)
            {
                separationProfile.BarcodeSeparationSettings = oBarcodeSeparationSettings;
                SettingChanged();
            }
        }

        private bool IsFormTypeExistence(string sFormType)
        {
            foreach (string docTypeName in m_FormTypes)
            {
                if (sFormType.Equals(docTypeName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private string GetBarcodeValue(BarcodeType barcodeType)
        {
            string sRet = "";
            switch (barcodeType)
            {
                case BarcodeType.bc128:
                    sRet = Barcode128;
                    break;

                case BarcodeType.bc39:
                    sRet = Barcode39;
                    break;

                case BarcodeType.bcIndustrial2of5:
                    sRet = BarcodeIndustrial2of5;
                    break;

                //case BarcodeType.bcInverted2of5:
                //    sRet = BarcodeInverted2of5;
                //    break;

                case BarcodeType.bcInterleaved2of5:
                    sRet = BarcodeInterleaved2of5;
                    break;

                case BarcodeType.bcIata2of5:
                    sRet = BarcodeIata2of5;
                    break;

                case BarcodeType.bcMatrix2of5:
                    sRet = BarcodeMatrix2of5;
                    break;

                //case BarcodeType.bcBcdMatrix:
                //    sRet = BarcodedMatrix;
                //    break;

                case BarcodeType.bcCodeabar:
                    sRet = BarcodeCodeabar;
                    break;

                case BarcodeType.bcDataLogic2of5:
                    sRet = BarcodeDataLogic2of5;
                    break;

                case BarcodeType.bcCODE93:
                    sRet = Barcode93;
                    break;

                case BarcodeType.bcEAN13:
                    sRet = BarcodeEAN13;
                    break;

                case BarcodeType.bcUPCA:
                    sRet = BarcodeUPCA;
                    break;

                case BarcodeType.bcEAN8:
                    sRet = BarcodeEAN8;
                    break;

                case BarcodeType.bcUPCE:
                    sRet = BarcodeUPCE;
                    break;

                case BarcodeType.bcADD5:
                    sRet = BarcodeADD5;
                    break;

                case BarcodeType.bcADD2:
                    sRet = BarcodeADD2;
                    break;
                case BarcodeType.bcQRcode:
                    sRet = BarcodeQR;
                    break;
            }
            return sRet;
        }

        private BarcodeType GetBarcodeType(string sBarcode)
        {
            switch (sBarcode)
            {
                case Barcode128:
                    return BarcodeType.bc128;

                case Barcode39:
                    return BarcodeType.bc39;

                case BarcodeIndustrial2of5:
                    return BarcodeType.bcIndustrial2of5;

                //case BarcodeInverted2of5:
                //    return BarcodeType.bcInverted2of5;

                case BarcodeInterleaved2of5:
                    return BarcodeType.bcInterleaved2of5;

                case BarcodeIata2of5:
                    return BarcodeType.bcIata2of5;

                case BarcodeMatrix2of5:
                    return BarcodeType.bcMatrix2of5;

                //case BarcodedMatrix:
                //    return BarcodeType.bcBcdMatrix;

                case BarcodeDataLogic2of5:
                    return BarcodeType.bcDataLogic2of5;

                case BarcodeCodeabar:
                    return BarcodeType.bcCodeabar;

                case Barcode93:
                    return BarcodeType.bcCODE93;

                case BarcodeEAN13:
                    return BarcodeType.bcEAN13;

                case BarcodeUPCA:
                    return BarcodeType.bcUPCA;

                case BarcodeEAN8:
                    return BarcodeType.bcEAN8;

                case BarcodeUPCE:
                    return BarcodeType.bcUPCE;

                case BarcodeADD5:
                    return BarcodeType.bcADD5;

                case BarcodeADD2:
                    return BarcodeType.bcADD2;

                case BarcodeQR:
                    return BarcodeType.bcQRcode;
            }

            return BarcodeType.bc39;
        }

        private string GetOperator(ComparisonType comparisonType)
        {
            string sRet = "";
            switch (comparisonType)
            {
                case ComparisonType.startwith:
                    sRet = OperatorStartWith;
                    break;
                case ComparisonType.contain:
                    sRet = OperatorContain;
                    break;
                case ComparisonType.endwith:
                    sRet = OperatorEndWith;
                    break;
                case ComparisonType.exact:
                    sRet = OperatorExact;
                    break;
            }
            return sRet;
        }

        private ComparisonType GetOperatorType(string sOperator)
        {
            if (sOperator == OperatorContain)
                return ComparisonType.contain;
            else if (sOperator == OperatorStartWith)
                return ComparisonType.startwith;
            else if (sOperator == OperatorEndWith)
                return ComparisonType.endwith;

            return ComparisonType.exact;
        }

        private bool IsNumeric(string sValue)
        {
            try { int.Parse(sValue); }
            catch { return false; }
            return true;
        }

        private string ValidateFixPageSetting()
        {
            if (cboFPFormType.SelectedItem == null)
                return "Fix Page Separation: The form type was not specified.";

            return "";
        }

        private string ValidateBlankPageSetting()
        {
            if (cboBPFormType.SelectedItem == null)
                return "Blank Page Separation: The form type was not specified.";

            return "";
        }

        private string ValidateBarcodeSeparationSetting(DataGridViewRow oRow)
        {
            for (int nIndex = 0; nIndex < 4; nIndex++)
            {
                if (oRow.Cells[nIndex].Value == null)
                    return "The current barcode separation setting is incomplete.";
            }

            //Minimum page count
            if (oRow.Cells[5].Value != null)
            {
                bool bCombineMinPage = Convert.ToBoolean(oRow.Cells[5].Value);
                if (bCombineMinPage)
                {
                    if (oRow.Cells[6].Value == null)
                    {
                        return "Minimum page count was not specified";
                    }
                    else
                    {
                        string sMinPageCount = oRow.Cells[6].Value.ToString();
                        if (!IsNumeric(sMinPageCount))
                            return "Minimum page count is not a numeric value.";
                        else if (int.Parse(sMinPageCount) <= 0)
                            return "Minimum page count must be greater than zero.";
                    }
                }
            }

            return "";
        }

        private BarcodeSeparationSetting PopulateBarcodeSeparationSetting(DataGridViewRow currentRow)
        {
            BarcodeSeparationSetting oSetting = new BarcodeSeparationSetting();
            oSetting.FormType = currentRow.Cells[0].Value.ToString();
            oSetting.eBarcode = GetBarcodeType(currentRow.Cells[1].Value.ToString());
            oSetting.eComparison = GetOperatorType(currentRow.Cells[2].Value.ToString());
            oSetting.CompareValue = currentRow.Cells[3].Value.ToString();

            if (currentRow.Cells[4].Value == null)
                oSetting.DiregardPage = false;
            else
                oSetting.DiregardPage = Convert.ToBoolean(currentRow.Cells[4].Value);

            if (currentRow.Cells[5].Value != null)
            {
                bool bCombine = Convert.ToBoolean(currentRow.Cells[5].Value);
                oSetting.CombineCheckWithMinimumPageCount = bCombine;
                if (bCombine)
                    oSetting.MinimumPageCount = int.Parse(currentRow.Cells[6].Value.ToString());
                else
                {
                    oSetting.MinimumPageCount = 0;
                    currentRow.Cells[6].Value = 0;
                }
            }

            return oSetting;
        }

        private void InitValue(SeparationProfile separationProfile)
        {
            m_SeparationProfile = new SeparationProfile();
            m_SeparationProfile.Active = separationProfile.Active;
            m_SeparationProfile.eSeparationType = separationProfile.eSeparationType;
            //m_SeparationProfile.BarcodeSeparationSettings = separationProfile.BarcodeSeparationSettings;
            foreach (BarcodeSeparationSetting oSetting in separationProfile.BarcodeSeparationSettings)
            {
                m_SeparationProfile.BarcodeSeparationSettings.Add(oSetting);
            }
            m_SeparationProfile.BlankPageSeparationSetting = separationProfile.BlankPageSeparationSetting;
            m_SeparationProfile.FixPageSeparationSetting = separationProfile.FixPageSeparationSetting;
        }

        private void UpdateCellValue(int nCellIndex, string sOldValue, string sNewValue)
        {
            foreach (DataGridViewRow oRow in dgvBarcode.Rows)
            {
                string cellValue = oRow.Cells[nCellIndex].FormattedValue.ToString();
                if (sOldValue.Equals(cellValue, StringComparison.OrdinalIgnoreCase))
                    oRow.Cells[nCellIndex].Value = sNewValue;
            }
        }

        private void UpdateDataGridViewComboItem(DataGridViewComboBoxColumn combo, string sOldValue, string sNewValue)
        {
            int nCounter = combo.Items.Count;
            for (int nIndex = 0; nIndex < nCounter; nIndex++)
            {
                if (sOldValue.Equals(combo.Items[nIndex].ToString(), StringComparison.OrdinalIgnoreCase))
                    combo.Items[nIndex] = sNewValue;
            }
        }

        private void RenameComboItem(ComboBox combo, string sOldValue, string sNewValue)
        {
            for (int nIndex = 0; nIndex < combo.Items.Count; nIndex++)
            {
                if (sOldValue.Equals(combo.Items[nIndex].ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    combo.Items[nIndex] = sNewValue;
                    break;
                }
            }
        }
        #endregion Private Methods

        #region Event-driven Methods
        private void cboFPFormType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                //m_SeparationProfile.eSeparationType = SeparationType.FixPage;
                //m_SeparationProfile.FixPageSeparationSetting.FormType = cboFPFormType.Text;
                SettingChanged();
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nudPageCount_ValueChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                //m_SeparationProfile.eSeparationType = SeparationType.FixPage;
                //m_SeparationProfile.FixPageSeparationSetting.PageCount = (int)nudPageCount.Value;
                SettingChanged();
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboBPFormType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                //m_SeparationProfile.eSeparationType = SeparationType.BlankPage;
                //m_SeparationProfile.BlankPageSeparationSetting.FormType = cboBPFormType.Text;
                SettingChanged();
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void nudThreshold_ValueChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                //m_SeparationProfile.eSeparationType = SeparationType.BlankPage;
                //m_SeparationProfile.BlankPageSeparationSetting.Threshold = (float)nudThreshold.Value;
                SettingChanged();
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkBPDeleteBlankPage_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                //m_SeparationProfile.eSeparationType = SeparationType.BlankPage;
                //m_SeparationProfile.BlankPageSeparationSetting.DiregardPage = chkBPDeleteBlankPage.Checked;
                SettingChanged();
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkEnabledSeparation_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                //m_SeparationProfile.Active = chkEnabledSeparation.Checked;

                grbSeparationType.Enabled = chkEnabledSeparation.Checked;
                grbSepDetail.Enabled = chkEnabledSeparation.Checked;
                if (chkEnabledSeparation.Checked)
                {
                    if (radFixPage.Checked)
                    {
                        //m_SeparationProfile.eSeparationType = SeparationType.FixPage;
                        tabSeparation.SelectedTab = tabPageFixPage;
                        panelFixPage.Enabled = true;
                        panelBlankPage.Enabled = false;
                        panelBarcode.Enabled = false;
                    }
                    else if (radBlankPage.Checked)
                    {
                        //m_SeparationProfile.eSeparationType = SeparationType.BlankPage;
                        tabSeparation.SelectedTab = tabPageBlankPage;
                        panelFixPage.Enabled = false;
                        panelBlankPage.Enabled = true;
                        panelBarcode.Enabled = false;
                    }
                    else if (radBarcode.Checked)
                    {
                        //m_SeparationProfile.eSeparationType = SeparationType.Barcode;
                        tabSeparation.SelectedTab = tabPageBarcode;
                        panelFixPage.Enabled = false;
                        panelBlankPage.Enabled = false;
                        panelBarcode.Enabled = true;
                    }
                    else
                    {
                        //m_SeparationProfile.eSeparationType = SeparationType.Custom;
                        tabSeparation.SelectedTab = tabPageCustom;
                        panelFixPage.Enabled = false;
                        panelBlankPage.Enabled = false;
                        panelBarcode.Enabled = false;
                    }
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SettingChanged();
            }
        }

        private void radFixPage_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                if (radFixPage.Checked)
                {
                    //m_SeparationProfile.eSeparationType = SeparationType.FixPage;
                    m_bLoading = true;
                    tabSeparation.SelectedTab = tabPageFixPage;
                    m_bLoading = false;
                    panelFixPage.Enabled = true;
                    panelBlankPage.Enabled = false;
                    panelBarcode.Enabled = false;
                    SettingChanged();
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radBlankPage_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                if (radBlankPage.Checked)
                {
                    //m_SeparationProfile.eSeparationType = SeparationType.BlankPage;
                    m_bLoading = true;
                    tabSeparation.SelectedTab = tabPageBlankPage;
                    m_bLoading = false;
                    panelFixPage.Enabled = false;
                    panelBlankPage.Enabled = true;
                    panelBarcode.Enabled = false;
                    SettingChanged();
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void radBarcode_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                if (radBarcode.Checked)
                {
                    //m_SeparationProfile.eSeparationType = SeparationType.Barcode;
                    m_bLoading = true;
                    tabSeparation.SelectedTab = tabPageBarcode;
                    m_bLoading = false;
                    panelFixPage.Enabled = false;
                    panelBlankPage.Enabled = false;
                    panelBarcode.Enabled = true;
                    SettingChanged();
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBarcode_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || m_bLoading)
                return;
            try
            {
                m_bBarcodeSeparationSettingChanged = true;

                //Combine With Min Page Count option was changed
                if (e.ColumnIndex == 5)
                {
                    DataGridViewCell cell = dgvBarcode.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (cell.Value != null)
                        dgvBarcode.Rows[e.RowIndex].Cells[6].ReadOnly = !(bool)cell.Value;
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SettingChanged();
            }
        }

        private void dgvBarcode_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                //m_SeparationProfile.BarcodeSeparationSettings.Add(new BarcodeSeparationSetting());
                SettingChanged();
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBarcode_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                if (MessageBox.Show(this, MessageAskDeleteRow, this.Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    //m_SeparationProfile.BarcodeSeparationSettings.RemoveAt(e.Row.Index);
                    SettingChanged();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBarcode_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (m_bLoading || !m_bBarcodeSeparationSettingChanged)
                return;
            try
            {
                DataGridViewRow currentRow = dgvBarcode.Rows[e.RowIndex];

                if (currentRow.IsNewRow)
                    return;

                string sRet = ValidateBarcodeSeparationSetting(currentRow);
                if (sRet == "")
                {
                    try
                    {
                        //m_SeparationProfile.eSeparationType = SeparationType.Barcode;
                        //BarcodeSeparationSetting oSetting = PopulateBarcodeSeparationSetting(currentRow);
                        //m_SeparationProfile.BarcodeSeparationSettings[e.RowIndex] = oSetting;
                        currentRow.ErrorText = "";
                        m_bBarcodeSeparationSettingChanged = false;
                    }
                    finally
                    {
                        SettingChanged();
                    }
                }
                else
                {
                    MessageBox.Show(this, sRet + "\r\n" + MessageValidation1, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    e.Cancel = true;
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvBarcode_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            dgvBarcode.Rows[e.RowIndex].ErrorText = e.Exception.Message;
        }

        private void dgvBarcode_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //not allowed to enter value of min page count for new row
                DataGridViewRow oRow = dgvBarcode.Rows[e.RowIndex];
                oRow.Cells[6].ReadOnly = !(bool)oRow.Cells[5].FormattedValue;
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tabSeparation_Selecting(object sender, TabControlCancelEventArgs e)
        {
            //Don't allow manual selection
            if(!m_bLoading)
                e.Cancel = true;
        }

        #endregion Event-driven Methods

        private void radCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bLoading)
                return;
            try
            {
                if (radCustom.Checked)
                {
                    m_bLoading = true;
                    tabSeparation.SelectedTab = tabPageCustom;
                    m_bLoading = false;
                    panelFixPage.Enabled = false;
                    panelBlankPage.Enabled = false;
                    panelBarcode.Enabled = false;
                    panelCustom.Enabled = true;
                    SettingChanged();
                }
            }
            catch (Exception oEx)
            {
                MessageBox.Show(this, oEx.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdCustomFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    textBoxCustomFile.Text = openFileDialog1.FileName;
                }
            }
            catch (Exception oExc)
            {
                MessageBox.Show(this, oExc.Message);
            }
        }
    }
}
