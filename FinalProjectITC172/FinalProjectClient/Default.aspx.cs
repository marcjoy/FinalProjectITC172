using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    ShowTrackerServiceReference.ShowTrackerServiceClient sc = new ShowTrackerServiceReference.ShowTrackerServiceClient();
    protected void Page_Load(object sender, EventArgs e)
    {
        //I hard coded the key in so I didn't have to do the login 
        //for this example
        Session["key"] = 2;
        if (!IsPostBack)
          
            FillVenueDropDown();
            FillArtistDropDown();
            FillShowDropDown();
            FillShowByVenueDropDown();
            FillShowByArtistDropDown();

    }
   
    
    protected void FillVenueDropDown()
    {
        string[] venues = sc.GetVenues();
        VenueDropDownList.DataSource = venues;
        VenueDropDownList.DataBind();
    }
    //artist dropdown
    protected void FillArtistDropDown()
    {
        string[] artist = sc.GetArtists();
        ArtistDropDownList.DataSource = artist;
        ArtistDropDownList.DataBind();
    }
    //show dropdown
    protected void FillShowDropDown()
    {
        string[] shows = sc.GetShows();
        ShowDropDownList.DataSource = shows;
        ShowDropDownList.DataBind();
    }
    //Show By Artist Dropdown
    protected void FillShowByArtistDropDown()
    {
        string[] artist = sc.GetArtists();
        ArtistByShowDropDownList.DataSource = artist;
        ArtistByShowDropDownList.DataBind();
    }
    //Show by Venue Dropdown
    protected void FillShowByVenueDropDown()
    {
        string[] venues = sc.GetVenues();
        VenueByShowDropDownList.DataSource = venues;
        VenueByShowDropDownList.DataBind();
    }

    //Buttons for Shows by Venues and for Shows by Artist
    protected void VenueByShowButton_Click(object sender, EventArgs e)
    {
        string venue = VenueByShowDropDownList.SelectedItem.Text;
        ShowTrackerServiceReference.ShowInfo[] show = sc.GetShowByVenue(venue);
        VenueByShowGridView.DataSource = show;
        VenueByShowGridView.DataBind();
    }

    protected void ArtistByShowButton_Click(object sender, EventArgs e)
    {
        string artist = ArtistByShowDropDownList.SelectedItem.Text;
        ShowTrackerServiceReference.ShowInfo[] show = sc.GetShowByArtist(artist);
        ArtistByShowGridView.DataSource = show;
        ArtistByShowGridView.DataBind();
    }

    protected void loginButton_Click(object sender, EventArgs e)
    {
        Login();
    }

    protected void Login()
    {
        ShowTrackerServiceReference.ShowTrackerServiceClient vl = new ShowTrackerServiceReference.ShowTrackerServiceClient();

        int key = vl.FanLogin(userTextBox.Text, passwordTextBox.Text);

        if (key != 0)
        {
            errorLabel.Text = "Welcome";
            Session["UserKey"] = key;
        }
        else
        {
            errorLabel.Text = "Invalid Login";
        }
    }
}