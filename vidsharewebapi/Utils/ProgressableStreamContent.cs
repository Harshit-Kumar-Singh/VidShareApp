using System.Net;

public class ProgressableStreamContent : HttpContent
{
    private const int defaultBufferSize = 4096;
    private readonly Stream content;
    private readonly int bufferSize;
    private readonly Action<long> progress;
    private readonly long totalSize;

    public ProgressableStreamContent(Stream content, int bufferSize, Action<long> progress, long totalSize)
    {
        this.content = content ?? throw new ArgumentNullException(nameof(content));
        this.bufferSize = bufferSize;
        this.progress = progress ?? throw new ArgumentNullException(nameof(progress));
        this.totalSize = totalSize;
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
    {
        var buffer = new byte[bufferSize];
        long uploaded = 0;
        int bytesRead;
        while ((bytesRead = await content.ReadAsync(buffer.AsMemory(0, buffer.Length))) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, bytesRead));
            uploaded += bytesRead;
            progress(uploaded);
        }
    }

    protected override bool TryComputeLength(out long length)
    {
        length = totalSize;
        return true;
    }
}
