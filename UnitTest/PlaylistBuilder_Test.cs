using System;
using System.Linq;
using UnitTest.Builders;
using Xunit;

namespace UnitTest
{
    public class PlaylistBuilder_Test
    {
        private readonly PlaylistBuilder _playlistBuilder = new PlaylistBuilder();

        [Fact]
        public void Should_Invoke_A_Error_List()
        {
            var playlists = this._playlistBuilder.GetPlaylistBuilderList();

            Assert.Null(playlists);
        }

        [Fact]
        public void Should_Invoke_A_EmptyList()
        {
            var playlists = this._playlistBuilder.GetPlaylistBuilderEmptyList();

            Assert.Empty(playlists);
        }

        [Fact]
        public void Should_Invoke_A_List()
        {
            var playlists = this._playlistBuilder.GetPlaylistBuilderList();

            Assert.NotEmpty(playlists);
        }

        [Fact]
        public void Should_Invoke_A_Single()
        {
            var weather = 10;
            var genre = "";

            if (weather < 10)
                genre = "decades";
            else if (weather >= 10 && weather < 25)
                genre = "rock";
            else
                genre = "pop";

            var playlist = this._playlistBuilder.GetPlaylistBuilderList().Where(p => p.Name.ToLower() == genre.ToLower()).FirstOrDefault();

            Assert.NotNull(playlist);
        }

        [Fact]
        public void Should_Invoke_A_Single_Error_Null()
        {
            var weather = 10;
            var genre = "";

            if (weather < 10)
                genre = "decades";
            else if (weather >= 10 && weather < 25)
                genre = "rock";
            else
                genre = "pop";

            var playlist = this._playlistBuilder.GetPlaylistBuilderList().Where(p => p.Name == genre).FirstOrDefault();

            Assert.NotNull(playlist);
        }
    }
}
