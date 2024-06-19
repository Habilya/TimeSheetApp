﻿namespace TimeSheetApp.Library.Providers;

public class GuidProvider : IGuidProvider
{
	public Guid NewGuid() => Guid.NewGuid();
}
