namespace CleanMinimalApi.Presentation.Serialization;

using System.Text.Json.Serialization;
using Application.Authors.Entities;
using Application.Movies.Entities;
using Application.Loans.Entities;
using Application.Versions.Entities;
using Requests;

/// <summary>
/// JSON serialization context for compile-time source generation.
/// Provides performance benefits and AOT (Ahead-of-Time) compilation support.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
    WriteIndented = false,
    GenerationMode = JsonSourceGenerationMode.Default)]
[JsonSerializable(typeof(Member))]
[JsonSerializable(typeof(List<Member>))]
[JsonSerializable(typeof(Book))]
[JsonSerializable(typeof(List<Book>))]
[JsonSerializable(typeof(Loan))]
[JsonSerializable(typeof(List<Loan>))]
[JsonSerializable(typeof(Version))]
[JsonSerializable(typeof(List<Version>))]
[JsonSerializable(typeof(CreateReviewRequest))]
[JsonSerializable(typeof(UpdateReviewRequest))]
public partial class AppJsonSerializerContext : JsonSerializerContext
{
}
