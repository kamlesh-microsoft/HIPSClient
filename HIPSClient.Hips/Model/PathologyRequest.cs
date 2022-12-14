using HIPSClient.Common.Tools.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPSClient.Hips.Model
{
  public enum ResultStatus
  {
    [EnumLiteral("F")]
    Final,
    [EnumLiteral("C")]
    Corrected,
    [EnumLiteral("X")]
    Delete
  }

  public class PathologyRequest
  {
    /// <summary>
    /// OBR-3
    /// </summary>
    public string OrderIdentifier { get; set; }

    /// <summary>
    /// OBR-2
    /// </summary>
    public string ReportIdentifier { get; set; }

    /// <summary>
    /// OBR-4
    /// </summary>
    public UniversalServiceIdentifier ReportName { get; set; }
    
    /// <summary>
    /// OBR-32
    /// </summary>
    public Provider DocumentAuthor { get; set; }    

    /// <summary>
    /// OBR-22 (When the report was released)
    /// </summary>
    public DateTimeOffset ReportedDateTime { get; set; }

    /// <summary>
    /// OBR-24 Diagnostic Serv Sect ID 0074 
    /// </summary>
    public string DepartmentCode { get; set; }

    /// <summary>
    /// OBR-25 ResultStatus
    /// </summary>
    public ResultStatus ReportStatus { get; set; }
  }
}
