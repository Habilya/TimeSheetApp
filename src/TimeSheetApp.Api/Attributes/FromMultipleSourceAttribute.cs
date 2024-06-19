using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TimeSheetApp.Api.Attributes;

public sealed class FromMultipleSourceAttribute : Attribute, IBindingSourceMetadata
{
	public BindingSource BindingSource { get; } = CompositeBindingSource.Create(
		new[] { BindingSource.Path, BindingSource.Query },
		nameof(FromMultipleSourceAttribute)
	);
}

