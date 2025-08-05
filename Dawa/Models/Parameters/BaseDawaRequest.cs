using System.Collections.Generic;

namespace Dawa.Models.Parameters;

public abstract class BaseDawaRequest
{
    public abstract Dictionary<string, string?> ToQueryParameters();
}