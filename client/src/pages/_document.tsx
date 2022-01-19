import Document, {
  DocumentContext,
  Head,
  Html,
  Main,
  NextScript,
} from "next/document";

const _document = () => {
  return (
    <Html>
      <Head>
        <link rel="shortcut icon" href="/favicon.ico" />

        {/* fonts */}
        <link
          rel="stylesheet"
          href="https://fonts.googleapis.com/css?family=Roboto:400,400i,500,500i,700,700i"
        />
      </Head>
      <body>
        <Main></Main>
        <NextScript />
      </body>
    </Html>
  );
};

export default _document;
