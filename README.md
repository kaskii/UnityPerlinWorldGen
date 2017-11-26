Minecraft-like world generation in Unity using builtin Mathf.PerlinNoise function. Features:

* World divided to 16x16 sized chunks
* Only chunks around the player are being generated on demand
* Only draws outer faces - even between chunks (skips sides and bottom)
* Multithreading chunk generation
* Object pooling for chunks

![Example](https://i.imgur.com/tR4BDkc.png)

![Chunk generation in profiler](https://i.imgur.com/VrlRuKI.png)
