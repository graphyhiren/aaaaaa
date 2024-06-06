using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.BlogUI;

public partial class BlogModule : SiteModuleControl
{
	protected BlogConfiguration config = new();
	//private int countOfDrafts = 0;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		EnableViewState = false;
	}

	protected virtual void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{

		postList.ModuleId = ModuleId;
		postList.PageId = PageId;
		postList.IsEditable = IsEditable;
		postList.Config = config;
		postList.SiteRoot = SiteRoot;
		postList.ImageSiteRoot = ImageSiteRoot;

	}

	protected virtual void PopulateLabels()
	{
		Title1.EditUrl = "Blog/EditPost.aspx";
		Title1.EditText = BlogResources.BlogAddPostLabel;

		//if ((IsEditable) && (countOfDrafts > 0))
		//{
		//    //BlogEditCategoriesLabel
		//    Title1.LiteralExtraMarkup = 
		//        "&nbsp;<a href='"
		//        + SiteRoot
		//        + "/Blog/EditCategory.aspx?pageid=" + PageId.ToInvariantString()
		//        + "&amp;mid=" + ModuleId.ToInvariantString()
		//        + "' class='ModuleEditLink' title='" + BlogResources.BlogEditCategoriesLabel + "'>" + BlogResources.BlogEditCategoriesLabel + "</a>"
		//        + "&nbsp;<a href='"
		//        + SiteRoot
		//        + "/Blog/Drafts.aspx?pageid=" + PageId.ToInvariantString()
		//        + "&amp;mid=" + ModuleId.ToInvariantString()
		//        + "' class='ModuleEditLink' title='" + BlogResources.BlogDraftsLink + "'>" + BlogResources.BlogDraftsLink + "</a>";
		//}
		//else 
		if (IsEditable)
		{
			Title1.LiteralExtraMarkup =
				Invariant($"&nbsp;<a href=\"{"Blog/Manage.aspx".ToLinkBuilder().PageId(PageId).ModuleId(ModuleId)}\" class=\"ModuleEditLink\" title=\"{BlogResources.Administration}\">{BlogResources.Administration}</a>");
		}
	}

	private void AddWindowsLiveWriterManifestLink()
	{
		// the manifest tells windows live writer about the capabilities of our metaweblogapi
		// http://msdn.microsoft.com/en-us/library/bb463263.aspx

		if (Page.Header != null)
		{
			Control c = Page.Header.FindControl("wlwmanifest");
			if (c == null)
			{
				Literal lnkManifest = new Literal();
				lnkManifest.ID = "wlwmanifest";
				lnkManifest.Text = "<link rel=\"wlwmanifest\" type=\"application/wlwmanifest+xml\" href=\""
					+ WebUtils.GetRelativeSiteRoot() + "/wlwmanifest.xml\" />";

				Page.Header.Controls.Add(lnkManifest);
			}
		}
	}


	protected virtual void LoadSettings()
	{
		//pnlContainer.ModuleId = ModuleId;
		config = new BlogConfiguration(Settings);
		if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}

		if (!WebConfigSettings.DisableMetaWeblogApi)
		{
			AddWindowsLiveWriterManifestLink();
		}

		searchBoxTop.Visible = config.ShowBlogSearchBox && !displaySettings.HideSearchBoxInPostList
			&& !displaySettings.ShowSearchInNav;


		//if (IsEditable)
		//{
		//    countOfDrafts = Blog.CountOfDrafts(ModuleId);
		//}
	}
}
