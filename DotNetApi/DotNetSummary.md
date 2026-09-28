Tooling and project setup

- The dotnet CLI does the jobs of node, npm and tsc together. NuGet is the .NET equivalent of npm, and a .csproj file does the jobs of package.json and tsconfig.json.
- Solution files (.sln / .slnx) group .NET projects only. Angular sits beside the API as a sibling folder, outside the solution.
- One repo for both suits this project. Separate repos make sense when teams or release schedules are separate, not because an app is large.
- .gitignore files apply to their folder and everything below it. git check-ignore -v <path> shows which rule ignores a path.
- The Angular dev proxy (proxy.conf.json plus proxyConfig in angular.json) forwards /api/* to .NET. The browser only ever talks to :4200, so no CORS setup is needed. The proxy only exists in development.
- HTTP locally, HTTPS in production. In production, HTTPS usually ends at a load balancer, so the app needs UseForwardedHeaders(). Know what UseHttpsRedirection() and UseHsts() do.
- dotnet watch -lp http is the .NET equivalent of nodemon. Some edits need a restart (Ctrl+R).

How ASP.NET Core is structured

- Program.cs has two phases. Before builder.Build(), you register services in the DI container. After it, you set up the middleware pipeline, where order matters, as in Express.
- Attributes ([ApiController], [Route], [HttpGet]) are like decorators. Each one applies to the next declaration, and has no {} body of its own.
- An endpoint is a public method on a controller. The class route is the prefix and the method route is added to it, e.g. [Route("api/[controller]")] + [HttpGet("{days}")]. Route parameters bind to method parameters by name.
- ActionResult<T> can return either T or an error such as BadRequest(...). Return T directly (without Ok()) so the compiler checks the type.
- With [ApiController], a failed type conversion (/abc) automatically returns 400. {days:int} in the route would give 404 instead.

C# language points

- Namespaces are independent of folders. The compiler builds every .cs file in the project together; there are no file-path imports. namespace X; declares a namespace; using X; imports one. Moving a file breaks nothing, but the convention is for namespaces to match folders.
- Namespaces and types share names. A namespace called WeatherForecast clashed with the class of the same name (CS0101).
- Constructor injection: a private readonly field set from a constructor parameter, or a primary constructor (whose parameters aren't readonly).
- const with PascalCase names for fixed values. Fields go at the top of the class.
- Guard clauses: check the invalid case, return early, leave the success path unindented. Invert conditions carefully (De Morgan: flip each comparison and swap &&/||), and test the boundaries and the values just outside them.
- In services, guards throw instead (ArgumentOutOfRangeException.ThrowIfLessThan, etc.).
- $"..." is the equivalent of a JavaScript template literal.

Dependency injection and lifetimes

- Depend on interfaces and register them in Program.cs: AddScoped<IService, Service>(). Never call new on your dependencies.
- Singleton: one instance for the app. Must be thread-safe, because ASP.NET handles requests on multiple threads, unlike Node.
- Scoped: one instance per request, shared by every service in that request.
- Transient: a new instance every time it's requested.
- Lifetime is about correctness (state, thread safety, dependencies), not performance. It controls reuse of the object, not caching of results.
- Captive dependency: a longer-lived service must not depend on a shorter-lived one. A singleton holding a scoped DbContext is the classic mistake.
- DbContext is a unit of work: a change tracker plus an identity map for one operation, with queries run one after another, never in parallel. The connection pool is what's shared across the app.
- Why scoped suits DbContext: services in the same request share one context, so a single SaveChanges() is one transaction. With transient, each service would have its own context, so changes wouldn't be saved atomically.
- Dispose versus garbage collection: Dispose() runs at a known moment when the scope ends; memory is reclaimed later. Disposable transients live until their scope ends, which is a leak risk when resolved from the root container.
- IDbContextFactory<T> lets a singleton create a short-lived DbContext when it needs one.
