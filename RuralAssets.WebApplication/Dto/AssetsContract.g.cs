// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: assets_contract.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace AElf.Contracts.Assets {

  /// <summary>Holder for reflection information generated from assets_contract.proto</summary>
  public static partial class AssetsContractReflection {

    #region Descriptor
    /// <summary>File descriptor for assets_contract.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AssetsContractReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVhc3NldHNfY29udHJhY3QucHJvdG8SBkFzc2V0cxoPYWVsZi9jb3JlLnBy",
            "b3RvGhJhZWxmL29wdGlvbnMucHJvdG8aG2dvb2dsZS9wcm90b2J1Zi9lbXB0",
            "eS5wcm90bxoeZ29vZ2xlL3Byb3RvYnVmL3dyYXBwZXJzLnByb3RvGh9nb29n",
            "bGUvcHJvdG9idWYvdGltZXN0YW1wLnByb3RvIngKCUFzc2V0SW5mbxIMCgRu",
            "YW1lGAEgASgJEg8KB2lkX2NhcmQYAiABKAkSEgoKYXNzZXRfdHlwZRgDIAEo",
            "BRIVCg1hc3NldF9pZF9saXN0GAQgAygFEiEKCmFzc2V0X2xpc3QYBSADKAsy",
            "DS5Bc3NldHMuQXNzZXQiuQEKBUFzc2V0EhAKCGFzc2V0X2lkGAEgASgFEg4K",
            "BnN0YXR1cxgCIAEoCRIPCgdiYW5rX2lkGAMgASgJEhMKC2xvYW5fYW1vdW50",
            "GAQgASgDEiwKCGR1ZV9kYXRlGAUgASgLMhouZ29vZ2xlLnByb3RvYnVmLlRp",
            "bWVzdGFtcBIRCglsb2FuX3JhdGUYBiABKAMSFgoObG9hbl9hZ3JlZW1lbnQY",
            "ByABKAwSDwoHaWRfY2FyZBgIIAEoCSI4ChFHZXRBc3NldEluZm9JbnB1dBIP",
            "CgdpZF9jYXJkGAEgASgJEhIKCmFzc2V0X3R5cGUYAiABKAUyhwMKDkFzc2V0",
            "c0NvbnRyYWN0Ej4KCkluaXRpYWxpemUSFi5nb29nbGUucHJvdG9idWYuRW1w",
            "dHkaFi5nb29nbGUucHJvdG9idWYuRW1wdHkiABI7CgxTZXRBc3NldEluZm8S",
            "ES5Bc3NldHMuQXNzZXRJbmZvGhYuZ29vZ2xlLnByb3RvYnVmLkVtcHR5IgAS",
            "MwoIU2V0QXNzZXQSDS5Bc3NldHMuQXNzZXQaFi5nb29nbGUucHJvdG9idWYu",
            "RW1wdHkiABJDCgxHZXRBc3NldEluZm8SGS5Bc3NldHMuR2V0QXNzZXRJbmZv",
            "SW5wdXQaES5Bc3NldHMuQXNzZXRJbmZvIgWIifcBARJOChdHZXRBc3NldElu",
            "Zm9XaXRoRGV0YWlscxIZLkFzc2V0cy5HZXRBc3NldEluZm9JbnB1dBoRLkFz",
            "c2V0cy5Bc3NldEluZm8iBYiJ9wEBGi6yzPYBKUFFbGYuQ29udHJhY3RzLkFz",
            "c2V0cy5Bc3NldHNDb250cmFjdFN0YXRlQhiqAhVBRWxmLkNvbnRyYWN0cy5B",
            "c3NldHNiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::AElf.Types.CoreReflection.Descriptor, global::AElf.OptionsReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.EmptyReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.WrappersReflection.Descriptor, global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::AElf.Contracts.Assets.AssetInfo), global::AElf.Contracts.Assets.AssetInfo.Parser, new[]{ "Name", "IdCard", "AssetType", "AssetIdList", "AssetList" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::AElf.Contracts.Assets.Asset), global::AElf.Contracts.Assets.Asset.Parser, new[]{ "AssetId", "Status", "BankId", "LoanAmount", "DueDate", "LoanRate", "LoanAgreement", "IdCard" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::AElf.Contracts.Assets.GetAssetInfoInput), global::AElf.Contracts.Assets.GetAssetInfoInput.Parser, new[]{ "IdCard", "AssetType" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class AssetInfo : pb::IMessage<AssetInfo> {
    private static readonly pb::MessageParser<AssetInfo> _parser = new pb::MessageParser<AssetInfo>(() => new AssetInfo());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AssetInfo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AElf.Contracts.Assets.AssetsContractReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AssetInfo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AssetInfo(AssetInfo other) : this() {
      name_ = other.name_;
      idCard_ = other.idCard_;
      assetType_ = other.assetType_;
      assetIdList_ = other.assetIdList_.Clone();
      assetList_ = other.assetList_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AssetInfo Clone() {
      return new AssetInfo(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "id_card" field.</summary>
    public const int IdCardFieldNumber = 2;
    private string idCard_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string IdCard {
      get { return idCard_; }
      set {
        idCard_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "asset_type" field.</summary>
    public const int AssetTypeFieldNumber = 3;
    private int assetType_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int AssetType {
      get { return assetType_; }
      set {
        assetType_ = value;
      }
    }

    /// <summary>Field number for the "asset_id_list" field.</summary>
    public const int AssetIdListFieldNumber = 4;
    private static readonly pb::FieldCodec<int> _repeated_assetIdList_codec
        = pb::FieldCodec.ForInt32(34);
    private readonly pbc::RepeatedField<int> assetIdList_ = new pbc::RepeatedField<int>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<int> AssetIdList {
      get { return assetIdList_; }
    }

    /// <summary>Field number for the "asset_list" field.</summary>
    public const int AssetListFieldNumber = 5;
    private static readonly pb::FieldCodec<global::AElf.Contracts.Assets.Asset> _repeated_assetList_codec
        = pb::FieldCodec.ForMessage(42, global::AElf.Contracts.Assets.Asset.Parser);
    private readonly pbc::RepeatedField<global::AElf.Contracts.Assets.Asset> assetList_ = new pbc::RepeatedField<global::AElf.Contracts.Assets.Asset>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::AElf.Contracts.Assets.Asset> AssetList {
      get { return assetList_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AssetInfo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AssetInfo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (IdCard != other.IdCard) return false;
      if (AssetType != other.AssetType) return false;
      if(!assetIdList_.Equals(other.assetIdList_)) return false;
      if(!assetList_.Equals(other.assetList_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (IdCard.Length != 0) hash ^= IdCard.GetHashCode();
      if (AssetType != 0) hash ^= AssetType.GetHashCode();
      hash ^= assetIdList_.GetHashCode();
      hash ^= assetList_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      if (IdCard.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(IdCard);
      }
      if (AssetType != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(AssetType);
      }
      assetIdList_.WriteTo(output, _repeated_assetIdList_codec);
      assetList_.WriteTo(output, _repeated_assetList_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (IdCard.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(IdCard);
      }
      if (AssetType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AssetType);
      }
      size += assetIdList_.CalculateSize(_repeated_assetIdList_codec);
      size += assetList_.CalculateSize(_repeated_assetList_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AssetInfo other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.IdCard.Length != 0) {
        IdCard = other.IdCard;
      }
      if (other.AssetType != 0) {
        AssetType = other.AssetType;
      }
      assetIdList_.Add(other.assetIdList_);
      assetList_.Add(other.assetList_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Name = input.ReadString();
            break;
          }
          case 18: {
            IdCard = input.ReadString();
            break;
          }
          case 24: {
            AssetType = input.ReadInt32();
            break;
          }
          case 34:
          case 32: {
            assetIdList_.AddEntriesFrom(input, _repeated_assetIdList_codec);
            break;
          }
          case 42: {
            assetList_.AddEntriesFrom(input, _repeated_assetList_codec);
            break;
          }
        }
      }
    }

  }

  public sealed partial class Asset : pb::IMessage<Asset> {
    private static readonly pb::MessageParser<Asset> _parser = new pb::MessageParser<Asset>(() => new Asset());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Asset> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AElf.Contracts.Assets.AssetsContractReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Asset() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Asset(Asset other) : this() {
      assetId_ = other.assetId_;
      status_ = other.status_;
      bankId_ = other.bankId_;
      loanAmount_ = other.loanAmount_;
      dueDate_ = other.dueDate_ != null ? other.dueDate_.Clone() : null;
      loanRate_ = other.loanRate_;
      loanAgreement_ = other.loanAgreement_;
      idCard_ = other.idCard_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Asset Clone() {
      return new Asset(this);
    }

    /// <summary>Field number for the "asset_id" field.</summary>
    public const int AssetIdFieldNumber = 1;
    private int assetId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int AssetId {
      get { return assetId_; }
      set {
        assetId_ = value;
      }
    }

    /// <summary>Field number for the "status" field.</summary>
    public const int StatusFieldNumber = 2;
    private string status_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Status {
      get { return status_; }
      set {
        status_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "bank_id" field.</summary>
    public const int BankIdFieldNumber = 3;
    private string bankId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string BankId {
      get { return bankId_; }
      set {
        bankId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "loan_amount" field.</summary>
    public const int LoanAmountFieldNumber = 4;
    private long loanAmount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long LoanAmount {
      get { return loanAmount_; }
      set {
        loanAmount_ = value;
      }
    }

    /// <summary>Field number for the "due_date" field.</summary>
    public const int DueDateFieldNumber = 5;
    private global::Google.Protobuf.WellKnownTypes.Timestamp dueDate_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.WellKnownTypes.Timestamp DueDate {
      get { return dueDate_; }
      set {
        dueDate_ = value;
      }
    }

    /// <summary>Field number for the "loan_rate" field.</summary>
    public const int LoanRateFieldNumber = 6;
    private long loanRate_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long LoanRate {
      get { return loanRate_; }
      set {
        loanRate_ = value;
      }
    }

    /// <summary>Field number for the "loan_agreement" field.</summary>
    public const int LoanAgreementFieldNumber = 7;
    private pb::ByteString loanAgreement_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString LoanAgreement {
      get { return loanAgreement_; }
      set {
        loanAgreement_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "id_card" field.</summary>
    public const int IdCardFieldNumber = 8;
    private string idCard_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string IdCard {
      get { return idCard_; }
      set {
        idCard_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Asset);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Asset other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AssetId != other.AssetId) return false;
      if (Status != other.Status) return false;
      if (BankId != other.BankId) return false;
      if (LoanAmount != other.LoanAmount) return false;
      if (!object.Equals(DueDate, other.DueDate)) return false;
      if (LoanRate != other.LoanRate) return false;
      if (LoanAgreement != other.LoanAgreement) return false;
      if (IdCard != other.IdCard) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (AssetId != 0) hash ^= AssetId.GetHashCode();
      if (Status.Length != 0) hash ^= Status.GetHashCode();
      if (BankId.Length != 0) hash ^= BankId.GetHashCode();
      if (LoanAmount != 0L) hash ^= LoanAmount.GetHashCode();
      if (dueDate_ != null) hash ^= DueDate.GetHashCode();
      if (LoanRate != 0L) hash ^= LoanRate.GetHashCode();
      if (LoanAgreement.Length != 0) hash ^= LoanAgreement.GetHashCode();
      if (IdCard.Length != 0) hash ^= IdCard.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (AssetId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(AssetId);
      }
      if (Status.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(Status);
      }
      if (BankId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(BankId);
      }
      if (LoanAmount != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(LoanAmount);
      }
      if (dueDate_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(DueDate);
      }
      if (LoanRate != 0L) {
        output.WriteRawTag(48);
        output.WriteInt64(LoanRate);
      }
      if (LoanAgreement.Length != 0) {
        output.WriteRawTag(58);
        output.WriteBytes(LoanAgreement);
      }
      if (IdCard.Length != 0) {
        output.WriteRawTag(66);
        output.WriteString(IdCard);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (AssetId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AssetId);
      }
      if (Status.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Status);
      }
      if (BankId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(BankId);
      }
      if (LoanAmount != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(LoanAmount);
      }
      if (dueDate_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(DueDate);
      }
      if (LoanRate != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(LoanRate);
      }
      if (LoanAgreement.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(LoanAgreement);
      }
      if (IdCard.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(IdCard);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Asset other) {
      if (other == null) {
        return;
      }
      if (other.AssetId != 0) {
        AssetId = other.AssetId;
      }
      if (other.Status.Length != 0) {
        Status = other.Status;
      }
      if (other.BankId.Length != 0) {
        BankId = other.BankId;
      }
      if (other.LoanAmount != 0L) {
        LoanAmount = other.LoanAmount;
      }
      if (other.dueDate_ != null) {
        if (dueDate_ == null) {
          DueDate = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        DueDate.MergeFrom(other.DueDate);
      }
      if (other.LoanRate != 0L) {
        LoanRate = other.LoanRate;
      }
      if (other.LoanAgreement.Length != 0) {
        LoanAgreement = other.LoanAgreement;
      }
      if (other.IdCard.Length != 0) {
        IdCard = other.IdCard;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            AssetId = input.ReadInt32();
            break;
          }
          case 18: {
            Status = input.ReadString();
            break;
          }
          case 26: {
            BankId = input.ReadString();
            break;
          }
          case 32: {
            LoanAmount = input.ReadInt64();
            break;
          }
          case 42: {
            if (dueDate_ == null) {
              DueDate = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(DueDate);
            break;
          }
          case 48: {
            LoanRate = input.ReadInt64();
            break;
          }
          case 58: {
            LoanAgreement = input.ReadBytes();
            break;
          }
          case 66: {
            IdCard = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class GetAssetInfoInput : pb::IMessage<GetAssetInfoInput> {
    private static readonly pb::MessageParser<GetAssetInfoInput> _parser = new pb::MessageParser<GetAssetInfoInput>(() => new GetAssetInfoInput());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GetAssetInfoInput> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::AElf.Contracts.Assets.AssetsContractReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetAssetInfoInput() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetAssetInfoInput(GetAssetInfoInput other) : this() {
      idCard_ = other.idCard_;
      assetType_ = other.assetType_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetAssetInfoInput Clone() {
      return new GetAssetInfoInput(this);
    }

    /// <summary>Field number for the "id_card" field.</summary>
    public const int IdCardFieldNumber = 1;
    private string idCard_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string IdCard {
      get { return idCard_; }
      set {
        idCard_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "asset_type" field.</summary>
    public const int AssetTypeFieldNumber = 2;
    private int assetType_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int AssetType {
      get { return assetType_; }
      set {
        assetType_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GetAssetInfoInput);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GetAssetInfoInput other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (IdCard != other.IdCard) return false;
      if (AssetType != other.AssetType) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (IdCard.Length != 0) hash ^= IdCard.GetHashCode();
      if (AssetType != 0) hash ^= AssetType.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (IdCard.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(IdCard);
      }
      if (AssetType != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(AssetType);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (IdCard.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(IdCard);
      }
      if (AssetType != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AssetType);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GetAssetInfoInput other) {
      if (other == null) {
        return;
      }
      if (other.IdCard.Length != 0) {
        IdCard = other.IdCard;
      }
      if (other.AssetType != 0) {
        AssetType = other.AssetType;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            IdCard = input.ReadString();
            break;
          }
          case 16: {
            AssetType = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
