// DlgVersion.cpp : implementation file
//

#include "stdafx.h"
#include "AccessControl.h"
#include "DlgVersion.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CDlgVersion dialog


CDlgVersion::CDlgVersion(CWnd* pParent /* = NULL */, LLONG hLoginId /* = 0 */)
	: CDialog(CDlgVersion::IDD, pParent)
{
	//{{AFX_DATA_INIT(CDlgVersion)
		// NOTE: the ClassWizard will add member initialization here
	m_hLoginId = hLoginId;
	//}}AFX_DATA_INIT
}


void CDlgVersion::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(CDlgVersion)
	DDX_Control(pDX, IDC_VERSION_STATIC_INFO, m_staVersion);
	//}}AFX_DATA_MAP
}


BEGIN_MESSAGE_MAP(CDlgVersion, CDialog)
	//{{AFX_MSG_MAP(CDlgVersion)
	ON_BN_CLICKED(IDC_VERSION_BTN_UPDATE, OnVersionUpdate)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CDlgVersion message handlers

BOOL CDlgVersion::OnInitDialog() 
{
	CDialog::OnInitDialog();
	g_SetWndStaticText(this, "VersionDlg");
	
	// TODO: Add extra initialization here
	OnVersionUpdate();

	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}

void CDlgVersion::OnVersionUpdate() 
{
	// TODO: Add your control notification handler code here
	if (!m_hLoginId)
	{
		AfxMessageBox(CString("we haven't login a device yet!"));
		return;
	}

	int nRet = 0;
	DHDEV_VERSION_INFO stuInfo = {0};
	BOOL bRet = CLIENT_QueryDevState(m_hLoginId, DH_DEVSTATE_SOFTWARE, (char*)&stuInfo, sizeof(stuInfo), &nRet, 3000);
	if (bRet)
	{
		CString csVer;
		csVer.Format("Serial: %s\r\n\
SoftwareVersion: %s\r\n\
ReleaseTime: %04d-%02d-%02d\r\n",
			stuInfo.szDevSerialNo,
			stuInfo.szSoftWareVersion,
			((stuInfo.dwSoftwareBuildDate>>16) & 0xffff),
			((stuInfo.dwSoftwareBuildDate>>8) & 0xff),
			(stuInfo.dwSoftwareBuildDate & 0xff));
		m_staVersion.SetWindowText(csVer);
	} 
	else
	{
		CString csErr;
		csErr.Format("get Version failed with code : %08x...", CLIENT_GetLastError());
		AfxMessageBox(csErr);
	}
}
