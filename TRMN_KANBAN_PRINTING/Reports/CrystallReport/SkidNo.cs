// Decompiled with JetBrains decompiler
// Type: TRMN_KANBAN_PRINTING.Reports.CrystallReport.SkidNo
// Assembly: TRMN_KANBAN_PRINTING, Version=1.0.0.1, Culture=neutral, PublicKeyToken=null
// MVID: 40AE7FA5-74DD-492D-AB7C-9D9ABFBC7F4F
// Assembly location: C:\Users\sar.puttaraju.ah\Desktop\Kanban_Printing_app\TRMN_KANBAN_PRINTING.exe

using CrystalDecisions.CrystalReports.Engine;
using System.ComponentModel;

namespace TRMN_KANBAN_PRINTING.Reports.CrystallReport
{
  public class SkidNo : ReportClass
  {
    public override string ResourceName
    {
      get => "SkidNo.rpt";
      set
      {
      }
    }

    public override bool NewGenerator
    {
      get => true;
      set
      {
      }
    }

    public override string FullResourceName
    {
      get => "TRMN_KANBAN_PRINTING.Reports.CrystallReport.SkidNo.rpt";
      set
      {
      }
    }

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Section Section1 => this.ReportDefinition.Sections[0];

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Section Section2 => this.ReportDefinition.Sections[1];

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Section Section3 => this.ReportDefinition.Sections[2];

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Section Section4 => this.ReportDefinition.Sections[3];

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Section Section5 => this.ReportDefinition.Sections[4];
  }
}
