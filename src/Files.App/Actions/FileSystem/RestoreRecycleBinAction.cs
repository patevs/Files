﻿// Copyright (c) 2024 Files Community
// Licensed under the MIT License. See the LICENSE.

namespace Files.App.Actions
{
	internal sealed class RestoreRecycleBinAction : BaseUIAction, IAction
	{
		private readonly IContentPageContext context;

		public string Label
			=> "Restore".GetLocalizedResource();

		public string Description
			=> "RestoreRecycleBinDescription".GetLocalizedResource();

		public RichGlyph Glyph
			=> new(themedIconStyle: "App.ThemedIcons.RestoreDeleted");

		public override bool IsExecutable =>
			context.PageType is ContentPageTypes.RecycleBin &&
			context.SelectedItems.Any() &&
			UIHelpers.CanShowDialog;

		public RestoreRecycleBinAction()
		{
			context = Ioc.Default.GetRequiredService<IContentPageContext>();

			context.PropertyChanged += Context_PropertyChanged;
		}

		public async Task ExecuteAsync(object? parameter = null)
		{
			if (context.ShellPage is not null)
				await RecycleBinHelpers.RestoreSelectionRecycleBinAsync(context.ShellPage);
		}

		private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IContentPageContext.PageType):
				case nameof(IContentPageContext.SelectedItems):
					if (context.PageType is ContentPageTypes.RecycleBin)
						OnPropertyChanged(nameof(IsExecutable));
					break;
			}
		}
	}
}
