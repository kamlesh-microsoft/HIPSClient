﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIPSClient.Common.Tools.Enum;
using HIPSClient.Common.Tools.String;
using PeterPiper.Hl7.V2.Model;

namespace HIPSClient.Hips.Model
{
  public class ADT_A01
  {
    public Patient Patient { get; set; }
    public HospitalEncounter HospitalEncounter { get; set; }
    public string GetHL7Message()
    {
      var oHL7 = Creator.Message("2.4", "ADT", "A01");
      //Sending Application
      oHL7.Segment("MSH").Field(3).AsString = "HIPSClient";
      //Sending Facility
      oHL7.Segment("MSH").Field(4).AsString = Common.HIPS.HipsConfig.HospitalCode;
      //Receiving Application
      oHL7.Segment("MSH").Field(5).AsString = "HIPS";
      //Receiving Facility
      oHL7.Segment("MSH").Field(6).AsString = "ADHA";

      oHL7.Segment("MSH").Field(7).Convert.DateTime.SetDateTimeOffset(DateTimeOffset.Now, false);
      oHL7.Segment("MSH").Field(15).AsString = "AL";
      oHL7.Segment("MSH").Field(16).AsString = "NE";
      oHL7.Segment("MSH").Field(17).AsString = "AU";
      oHL7.Segment("MSH").Field(18).AsString = "ASCII";
      oHL7.Segment("MSH").Field(19).AsString = "EN";

      var EVN = Creator.Segment("EVN");
      oHL7.Add(EVN);
      EVN.Field(1).AsString = "A01";
      EVN.Field(1).AsString = oHL7.Segment("MSH").Field(6).AsString;

      

      var PID = Creator.Segment("PID");
      oHL7.Add(PID);
      PID.Field(1).AsString = "1";
      //StateIdentifier
      if (Patient.StateIdentifier != null)
      {
        if (Patient.StateIdentifier.Value.IsSet())
        {
          var PID2 = Creator.Field();
          PID2.Component(1).AsString = Patient.StateIdentifier.Value;
          PID2.Component(4).AsString = Patient.StateIdentifier.AssigningAuthority;
          PID2.Component(5).AsString = Patient.StateIdentifier.IdentifierTypeCode;
          PID.Element(2).Add(PID2);
        }
      }

      //Identifier List
      if (Patient.IdentifierList != null)
      {
        foreach (var Id in Patient.IdentifierList)
        {
          var PID3 = Creator.Field();
          if (Id.Type == Model.PatientIdentifierType.MedicalRecordNumber)
          {
            //HIPS Pads 9 char to the left with zeros for MRNs, so may as well do it here to be clear.
            PID3.Component(1).AsString = Id.Value.PadLeft(9, '0');
          }
          else
          {
            PID3.Component(1).AsString = Id.Value;
          }

          PID3.Component(4).AsString = Id.AssigningAuthority;
          PID3.Component(5).AsString = Id.IdentifierTypeCode;
          PID.Element(3).Add(PID3);
        }
      }

      //Name
      if (Patient.Family.IsSet())
        PID.Field(5).Component(1).AsString = Patient.Family;
      if (Patient.Given.IsSet())
        PID.Field(5).Component(2).AsString = Patient.Given;
      if (Patient.Title.IsSet())
        PID.Field(5).Component(5).AsString = Patient.Title;
      PID.Field(5).Component(7).AsString = "L";

      //DateTime of Birth
      PID.Field(7).Convert.DateTime.SetDateTimeOffset(new DateTimeOffset(Patient.DateOfBirth, DateTimeOffset.Now.Offset), false, PeterPiper.Hl7.V2.Support.Tools.DateTimeSupportTools.DateTimePrecision.Date);

      //Gender
      PID.Field(8).AsString = Patient.Gender.GetLiteral();

      //Race
      if (Patient.IndigenousStatus != null)
      {
        PID.Field(10).Component(1).AsString = Patient.IndigenousStatus.Code;
        PID.Field(10).Component(2).AsString = Patient.IndigenousStatus.Description;
        PID.Field(10).Component(3).AsString = Patient.IndigenousStatus.NameOfCodingSystem;
      }

      //Address
      if (Patient.Address != null)
      {
        if (Patient.Address.AddressLineOne.IsSet())
          PID.Field(11).Component(1).AsString = Patient.Address.AddressLineOne;
        if (Patient.Address.AddressLineTwo.IsSet())
          PID.Field(11).Component(2).AsString = Patient.Address.AddressLineTwo;
        if (Patient.Address.Suburb.IsSet())
          PID.Field(11).Component(3).AsString = Patient.Address.Suburb;
        if (Patient.Address.State.IsSet())
          PID.Field(11).Component(4).AsString = Patient.Address.State;
        if (Patient.Address.PostCode.IsSet())
          PID.Field(11).Component(5).AsString = Patient.Address.PostCode;
        if (Patient.Address.Country.IsSet())
          PID.Field(11).Component(6).AsString = Patient.Address.Country;
        PID.Field(11).Component(7).AsString = "H";
      }

      //Home Phone
      if (Patient.HomeContact != null)
      {
        if (Patient.HomeContact.Value.IsSet())
        {
          PID.Field(14).Component(2).AsString = "PRN";
          if (IsMobileNumber(Patient.HomeContact.Value))
          {
            PID.Field(14).Component(3).AsString = "CP";
          }
          else
          {
            PID.Field(14).Component(3).AsString = "PH";
          }
          PID.Field(14).Component(7).AsString = Patient.HomeContact.Value;
        }
      }

      //Work Phone
      if (Patient.WorkContact != null)
      {
        if (Patient.WorkContact.Value.IsSet())
        {
          PID.Field(14).Component(2).AsString = "WPN";
          if (IsMobileNumber(Patient.WorkContact.Value))
          {
            PID.Field(14).Component(3).AsString = "CP";
          }
          else
          {
            PID.Field(14).Component(3).AsString = "PH";
          }
          PID.Field(14).Component(7).AsString = Patient.WorkContact.Value;
        }
      }

      var PV1 = Creator.Segment("PV1");
      oHL7.Add(PV1);
      PV1.Field(2).AsString = "I";
      //Ward
      if (HospitalEncounter.Ward.IsSet())
        PV1.Field(3).Component(1).AsString = HospitalEncounter.Ward;
      //Room
      if (HospitalEncounter.Room.IsSet())
        PV1.Field(3).Component(2).AsString = HospitalEncounter.Room;
      //Bed
      if (HospitalEncounter.Bed.IsSet())
        PV1.Field(3).Component(3).AsString = HospitalEncounter.Bed;
      //Hospital Encounter Number
      if (HospitalEncounter.VisitNumber.IsSet())
        PV1.Field(19).AsString = HospitalEncounter.VisitNumber;
      //Admit Date
      if (HospitalEncounter.AdmissionDate.HasValue)
        PV1.Field(44).Convert.DateTime.SetDateTimeOffset(new DateTimeOffset(HospitalEncounter.AdmissionDate.Value, new TimeSpan(10, 0, 0)), false, PeterPiper.Hl7.V2.Support.Tools.DateTimeSupportTools.DateTimePrecision.Date);
      //Discharge Date
      if (HospitalEncounter.DischargeDate.HasValue)
        PV1.Field(44).Convert.DateTime.SetDateTimeOffset(new DateTimeOffset(HospitalEncounter.DischargeDate.Value, new TimeSpan(10, 0, 0)), false, PeterPiper.Hl7.V2.Support.Tools.DateTimeSupportTools.DateTimePrecision.Date);

      return oHL7.AsStringRaw;
    }

    private bool IsMobileNumber(string value)
    {
      return (value.RemoveWhitespace().StartsWith("04") || value.RemoveWhitespace().StartsWith("+614"));
    }

    

  }
}
