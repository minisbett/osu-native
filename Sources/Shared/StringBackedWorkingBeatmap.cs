// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.IO;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.Formats;
using osu.Game.IO;
using osu.Game.Skinning;

namespace osu.Native
{
    public class StringBackedWorkingBeatmap : WorkingBeatmap
    {
        private readonly IBeatmap beatmap;

        public StringBackedWorkingBeatmap(string text, int? beatmapId = null)
            : this(readFromString(text))
        {
            if (beatmapId.HasValue)
                beatmap.BeatmapInfo.OnlineID = beatmapId.Value;
        }

        public StringBackedWorkingBeatmap(IBeatmap beatmap)
            : base(beatmap.BeatmapInfo, null)
        {
            this.beatmap = beatmap;
        }

        private static Beatmap readFromString(string text)
        {
            using (var stream = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream, leaveOpen: true))
                    sw.Write(text);
                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new LineBufferedReader(stream))
                    return Decoder.GetDecoder<Beatmap>(reader).Decode(reader);
            }
        }

        protected override IBeatmap GetBeatmap() => beatmap;

        public override Texture GetBackground() => throw new NotImplementedException();

        public override Stream GetStream(string storagePath) => throw new NotImplementedException();

        protected override Track GetBeatmapTrack() => throw new NotImplementedException();

        protected override ISkin GetSkin() => throw new NotImplementedException();
    }
}
