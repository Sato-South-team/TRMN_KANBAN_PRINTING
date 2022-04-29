// Decompiled with JetBrains decompiler
// Type: TRMN_KANBAN_PRINTING.Reports.CrystallReport.CachedTrackingReport
// Assembly: TRMN_KANBAN_PRINTING, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 40AE7FA5-74DD-492D-AB7C-9D9ABFBC7F4F
// Assembly location: C:\Users\sar.puttaraju.ah\Desktop\Kanban_Printing_app\TRMN_KANBAN_PRINTING.exe

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using System;
using System.ComponentModel;
using System.Drawing;

namespace TRMN_KANBAN_PRINTING.Reports.CrystallReport
{
  [ToolboxBitmap(typeof (ExportOptions), "report.bmp")]
  public class CachedTrackingReport : Component, ICachedReport
  {
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool IsCacheable
    {
      get => true;
      set
      {
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual bool ShareDBLogonInfo
    {
      get => false;
      set
      {
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public virtual TimeSpan CacheTimeOut
    {
      get => CachedReportConstants.DEFAULT_TIMEOUT;
      set
      {
      }
    }

    public virtual ReportDocument CreateReport()
    {
      TrackingReport report = new TrackingReport();
      report.Site = this.Site;
      return (ReportDocument) report;
    }

    public virtual string GetCustomizedCacheKey(RequestContext request) => (string) null;
  }
}
