using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShowTrackerService" in code, svc and config file together.
public class ShowTrackerService : IShowTrackerService
{
    ShowTrackerEntities db = new ShowTrackerEntities();

    public int AddFanArtist(int fanKey, string artistName)
    {
        int result = 1;

        Fan myFan = (from f in db.Fans
                     where f.FanKey == fanKey
                     select f).First();

        Artist myArtist = (from a in db.Artists
                           where a.ArtistName.Equals(artistName)
                           select a).First();

        //add the artist to the fan;'s collection of artists
        myFan.Artists.Add(myArtist);

        //save the changes
        db.SaveChanges();

        return result;
    }

    public int FanLogin(string fanUsername, string fanPassword)
    {
        int fanKey = 0;
        int result = db.usp_FanLogin(fanUsername,fanPassword);
        if (result != -1)
        {
            var key = (from f in db.Fans
                       where f.FanName.Equals(fanUsername)
                       select new { f.FanKey }).FirstOrDefault();

            fanKey = key.FanKey;

        }
        return fanKey;

    }

    public List<string> GetArtists()
    {
        var artist = from a in db.Artists orderby a.ArtistName select new { a.ArtistName };

        List<string> aName = new List<string>();
        foreach (var a in artist)
        {
            aName.Add(a.ArtistName.ToString());
        }
        return aName;
    }

    public List<ShowInfo> GetShowByArtist(string ArtistName)
    {
        List<ShowInfo> artshow = new List<ShowInfo>();
        var show = from s in db.Shows
                   from a in s.ShowDetails
                   where a.Artist.ArtistName.Equals(ArtistName)
                   select new { s.ShowName, s.ShowDate, s.ShowTime, s.Venue.VenueName };
        foreach (var sa in show)
        {
            ShowInfo info = new ShowInfo();
            info.ShowName = sa.ShowName;
            info.ShowDate = sa.ShowDate.ToString();
            info.ShowTime = sa.ShowTime.ToString();
            info.VenueName = sa.VenueName;

            artshow.Add(info);

        }
        return artshow;
    }

    public List<ShowInfo> GetShowByVenue(string VenueName)
    {
        List<ShowInfo> venShow = new List<ShowInfo>();
        var vshow = from sh in db.Shows
                    from v in sh.ShowDetails
                    where v.Show.Venue.VenueName.Equals(VenueName)
                    select new { sh.ShowName, sh.ShowDate, sh.ShowTime, sh.Venue.VenueName };
        foreach (var sa in vshow)
        {
            ShowInfo info = new ShowInfo();
            info.ShowName = sa.ShowName;
            info.ShowDate = sa.ShowDate.ToString();
            info.ShowTime = sa.ShowTime.ToString();
            info.VenueName = sa.VenueName;

            venShow.Add(info);

        }
        return venShow;
    }

    public List<string> GetShows()
    {
        var shows = from s in db.Shows orderby s.ShowName select new { s.ShowName };

        List<string> sName = new List<string>();
        foreach (var s in shows)
        {
            sName.Add(s.ShowName.ToString());
        }
        return sName;
    }

    public List<string> GetVenues()
    {
        var venues = from v in db.Venues orderby v.VenueName select new { v.VenueName };

        List<string> vName = new List<string>();
        foreach (var v in venues)
        {
            vName.Add(v.VenueName.ToString());
        }
        return vName;
    }

    public bool RegisterFan(Fan f, FanLogin fl)
    {
        bool result = true;
        int pass = db.usp_RegisterFan(f.FanName, f.FanEmail, fl.FanLoginUserName, fl.FanLoginPasswordPlain);
        if(pass == -1)
        {
            result = false;
        }
        return result;
    }

    public List<ShowInfo> GetShowsForFanArtists(int fanKey)
    {
        //get the fan
        Fan myFan = (from f in db.Fans
                     where f.FanKey == fanKey
                     select f).First();

        List<ShowInfo> shows = new List<ShowInfo>();

        //this loop within a loop is very inefficient
        foreach (Artist a in myFan.Artists)
        {
            //get all the shows for the fan
            var shws = from s in db.Shows
                       from sd in s.ShowDetails
                       where sd.ArtistKey == a.ArtistKey
                       select new
                       {
                           s.ShowName,
                           s.ShowTime,
                           s.ShowDate,
                           s.ShowTicketInfo,
                           s.Venue.VenueName,
                           sd.Artist.ArtistName
                       };

            //loop through the shows and write them to 
            //ShowInfo objects then add those objects
            //to the list
            foreach (var sh in shws)
            {
                ShowInfo info = new ShowInfo();
                info.ShowName = sh.ShowName;
                info.ShowDate = sh.ShowDate.ToString();
                info.ShowTime = sh.ShowTime.ToString();
                info.TicketInfo = sh.ShowTicketInfo;
                info.VenueName = sh.VenueName;
                info.ArtistName = sh.ArtistName;

                shows.Add(info);
            }


        }
        return shows;

    }
}

   
   
