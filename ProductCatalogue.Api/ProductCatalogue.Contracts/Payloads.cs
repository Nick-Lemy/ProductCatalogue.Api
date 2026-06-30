namespace ProductCatalogue.Contracts;

public record AssetUploadedPayload(
    Guid AssetId,
    Guid ProductId,
    Guid? VariantId,
    string AssetType,
    string FileUrl);

public record AssetApprovedPayload(
    Guid AssetId,
    Guid ProductId);

public record AssetRejectedPayload(
    Guid AssetId,
    Guid ProductId,
    string RejectionReason);

public record ProductSubmittedForReviewPayload(
    Guid ProductId,
    string ProductCode,
    string Name);

public record ProductPublishedPayload(
    Guid ProductId,
    string ProductCode,
    string Name);
