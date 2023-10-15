import { Application, Router, isHttpError } from "https://deno.land/x/oak/mod.ts";

const router = new Router();

router.get("/api/comments", (ctx) => {
  const { searchParams } = new URL(ctx.request.url);
  const postUrl = searchParams.get('post_url');
  const pageStr = searchParams.get('page');
  const limit = searchParams.get('limit');

  // TODO: query from Kv

  ctx.response.body = { code: 0, message: 'ok', detail: [] };
  ctx.response.type = "json";
});

router.post("/api/comments", async (ctx) => {

  const body =  ctx.request.body();
  const comment = await body.value;
  console.log(comment);

  // TODO: add to Kv

  ctx.response.body = { code: 0, message: 'ok', detail: {} };
  ctx.response.type = "json";
});


const app = new Application();

// Logger
app.use(async (ctx, next) => {
  await next();
  const rt = ctx.response.headers.get("X-Response-Time");
  console.log(`${ctx.request.method} ${ctx.request.url} - ${rt}`);
});

// Timing
app.use(async (ctx, next) => {
  const start = Date.now();
  await next();
  const ms = Date.now() - start;
  ctx.response.headers.set("X-Response-Time", `${ms}`);
});

// Error handler
app.use(async (context, next) => {
  try {
    await next();
  } catch (err) {
    console.error(err);
    // context.response.status = 200
    const { message } = err;
    context.response.body = { code: 5, message };
    context.response.type = "json"
  }
});

app.use(router.routes());
app.use(router.allowedMethods());

await app.listen({ port: 8000 });