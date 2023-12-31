USE [inovapos_rev1202]
GO
/****** Object:  Table [dbo].[im_tstock]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_tstock](
	[tgl] [datetime] NOT NULL,
	[kd_barang] [varchar](20) NOT NULL,
	[qty] [int] NULL,
	[hpp] [numeric](18, 0) NULL,
	[uid] [varchar](50) NULL,
	[uid_edit] [varchar](50) NULL,
	[tgl_tambah] [datetime] NULL,
	[tgl_edit] [datetime] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_msatuan]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_msatuan](
	[kd_satuan] [char](3) NOT NULL,
	[nm_satuan] [varchar](20) NULL,
	[uid] [varchar](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [varchar](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_im_msatuan] PRIMARY KEY CLUSTERED 
(
	[kd_satuan] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mrak]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mrak](
	[kd_rak] [char](10) NOT NULL,
	[nm_rak] [varchar](30) NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mpemasok_cp]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mpemasok_cp](
	[kd_cp] [int] IDENTITY(1,1) NOT NULL,
	[kd_ps] [char](20) NULL,
	[nm_lengkap] [varchar](40) NULL,
	[jabatan] [varchar](30) NULL,
	[telp] [varchar](30) NULL,
	[hp] [varchar](30) NULL,
	[email] [varchar](30) NULL,
	[ket] [varchar](50) NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_im_mpemasok_cp] PRIMARY KEY CLUSTERED 
(
	[kd_cp] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mpemasok]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mpemasok](
	[kd_ps] [char](6) NOT NULL,
	[nm_ps] [varchar](30) NULL,
	[alamat] [varchar](60) NULL,
	[kota] [varchar](30) NULL,
	[pos] [char](5) NULL,
	[propinsi] [varchar](30) NULL,
	[telp] [varchar](30) NULL,
	[fax] [varchar](30) NULL,
	[email] [varchar](30) NULL,
	[web] [varchar](30) NULL,
	[cp] [varchar](30) NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_im_mpemasok] PRIMARY KEY CLUSTERED 
(
	[kd_ps] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mgudang]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mgudang](
	[kd_gudang] [char](6) NOT NULL,
	[nm_gudang] [varchar](50) NULL,
	[alamat] [varchar](50) NULL,
	[kota] [varchar](30) NULL,
	[pos] [char](5) NULL,
	[propinsi] [varchar](30) NULL,
	[telp] [varchar](20) NULL,
	[fax] [varchar](20) NULL,
	[email] [varchar](30) NULL,
	[hp] [varchar](50) NULL,
	[cp] [varchar](50) NULL,
	[uid] [varchar](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [varchar](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_im_mgudang] PRIMARY KEY CLUSTERED 
(
	[kd_gudang] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mgroup_barang]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mgroup_barang](
	[kd_group] [nvarchar](10) NOT NULL,
	[nm_group] [nvarchar](40) NULL,
	[uid] [varchar](20) NULL,
	[uid_edit] [varchar](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_im_mgroup_barang] PRIMARY KEY CLUSTERED 
(
	[kd_group] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mgroup]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mgroup](
	[kd_group] [char](10) NOT NULL,
	[nm_group] [varchar](30) NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_im_mgroup] PRIMARY KEY CLUSTERED 
(
	[kd_group] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mbarang_pemasok]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mbarang_pemasok](
	[kd_barang] [varchar](20) NOT NULL,
	[kd_ps] [char](6) NOT NULL,
	[kd_barang_pemasok] [varchar](20) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[im_mbarang]    Script Date: 10/31/2012 08:58:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[im_mbarang](
	[kd_barang] [char](20) NOT NULL,
	[barcode] [char](20) NULL,
	[kd_group] [char](10) NULL,
	[nm_barang] [varchar](40) NULL,
	[kd_satuan] [char](3) NULL,
	[harga_jual] [money] NULL,
	[hpp] [money] NULL,
	[kd_pemasok] [char](6) NULL,
	[stock_min] [int] NULL,
	[kd_rak] [char](10) NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_im_mbarang] PRIMARY KEY CLUSTERED 
(
	[kd_barang] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tretur_beli_dtl]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tretur_beli_dtl](
	[no_faktur] [char](20) NOT NULL,
	[no_faktur_beli] [char](20) NULL,
	[kd_barang] [char](20) NULL,
	[qty] [int] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tretur_beli]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tretur_beli](
	[no_faktur] [nchar](10) NOT NULL,
	[tgl] [datetime] NULL,
	[ket] [varchar](50) NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_topname_dtl]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_topname_dtl](
	[kd_opname] [char](20) NOT NULL,
	[kd_barang] [char](20) NULL,
	[qty] [int] NULL,
	[dihitung] [bit] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_topname]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_topname](
	[kd_opname] [char](20) NOT NULL,
	[tgl] [datetime] NULL,
	[ket] [varchar](50) NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tmutasi_masuk_dtl]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tmutasi_masuk_dtl](
	[no_faktur] [char](20) NOT NULL,
	[kd_barang] [char](20) NOT NULL,
	[qty] [int] NULL,
	[kd_satuan] [char](3) NULL,
	[harga] [money] NULL,
	[diskon] [money] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tmutasi_masuk]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tmutasi_masuk](
	[no_faktur] [char](20) NOT NULL,
	[tgl] [datetime] NULL,
	[kd_gudang] [char](6) NULL,
	[ket] [varchar](50) NULL,
	[kd_term] [char](3) NULL,
	[diskon] [money] NULL,
	[biaya_kirim] [money] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tmutasi_keluar_dtl]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tmutasi_keluar_dtl](
	[no_faktur] [char](20) NOT NULL,
	[kd_barang] [char](20) NOT NULL,
	[qty] [int] NULL,
	[kd_satuan] [char](3) NULL,
	[harga] [money] NULL,
	[diskon] [money] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tmutasi_keluar]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tmutasi_keluar](
	[no_faktur] [char](20) NOT NULL,
	[tgl] [datetime] NULL,
	[kd_gudang] [char](6) NULL,
	[ket] [varchar](50) NULL,
	[kd_term] [char](3) NULL,
	[diskon] [money] NULL,
	[biaya_kirim] [money] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tbeli_dtl]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tbeli_dtl](
	[no_faktur] [char](20) NOT NULL,
	[kd_barang] [char](20) NOT NULL,
	[qty] [int] NULL,
	[kd_satuan] [char](3) NULL,
	[harga] [money] NULL,
	[diskon] [money] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_ac_tbeli_dtl] PRIMARY KEY CLUSTERED 
(
	[no_faktur] ASC,
	[kd_barang] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ac_tbeli]    Script Date: 10/31/2012 08:58:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ac_tbeli](
	[no_faktur] [char](20) NOT NULL,
	[tgl] [datetime] NULL,
	[kd_ps] [char](6) NULL,
	[ket] [varchar](50) NULL,
	[kd_term] [char](3) NULL,
	[diskon] [money] NULL,
	[biaya_kirim] [money] NULL,
	[uid] [char](20) NULL,
	[tgl_tambah] [datetime] NULL,
	[uid_edit] [char](20) NULL,
	[tgl_edit] [datetime] NULL,
 CONSTRAINT [PK_ac_tbeli] PRIMARY KEY CLUSTERED 
(
	[no_faktur] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_ac_tbeli_tgl_tambah]    Script Date: 10/31/2012 08:58:53 ******/
ALTER TABLE [dbo].[ac_tbeli] ADD  CONSTRAINT [DF_ac_tbeli_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_ac_tbeli_tgl_edit]    Script Date: 10/31/2012 08:58:53 ******/
ALTER TABLE [dbo].[ac_tbeli] ADD  CONSTRAINT [DF_ac_tbeli_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
/****** Object:  Default [DF_im_mbarang_tgl_tambah]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mbarang] ADD  CONSTRAINT [DF_im_mbarang_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_im_mbarang_tgl_edit]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mbarang] ADD  CONSTRAINT [DF_im_mbarang_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
/****** Object:  Default [DF_im_mgroup_tgl_tambah]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mgroup] ADD  CONSTRAINT [DF_im_mgroup_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_im_mgroup_tgl_edit]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mgroup] ADD  CONSTRAINT [DF_im_mgroup_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
/****** Object:  Default [DF_im_mgroup_barang_tgl_tambah]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mgroup_barang] ADD  CONSTRAINT [DF_im_mgroup_barang_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_im_mgroup_barang_tgl_edit]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mgroup_barang] ADD  CONSTRAINT [DF_im_mgroup_barang_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
/****** Object:  Default [DF_im_mgudang_tgl_tambah]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mgudang] ADD  CONSTRAINT [DF_im_mgudang_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_im_mgudang_tgl_edit]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mgudang] ADD  CONSTRAINT [DF_im_mgudang_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
/****** Object:  Default [DF_im_mpemasok_tgl_tambah]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mpemasok] ADD  CONSTRAINT [DF_im_mpemasok_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_im_mpemasok_tgl_edit]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mpemasok] ADD  CONSTRAINT [DF_im_mpemasok_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
/****** Object:  Default [DF_im_mpemasok_cp_tgl_tambah]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mpemasok_cp] ADD  CONSTRAINT [DF_im_mpemasok_cp_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_im_mpemasok_cp_tgl_edit]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_mpemasok_cp] ADD  CONSTRAINT [DF_im_mpemasok_cp_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
/****** Object:  Default [DF_im_msatuan_tgl_tambah]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_msatuan] ADD  CONSTRAINT [DF_im_msatuan_tgl_tambah]  DEFAULT (getdate()) FOR [tgl_tambah]
GO
/****** Object:  Default [DF_im_msatuan_tgl_edit]    Script Date: 10/31/2012 08:58:54 ******/
ALTER TABLE [dbo].[im_msatuan] ADD  CONSTRAINT [DF_im_msatuan_tgl_edit]  DEFAULT (getdate()) FOR [tgl_edit]
GO
