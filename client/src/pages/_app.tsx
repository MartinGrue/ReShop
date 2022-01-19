import "../scss/index.scss";

import type { AppProps } from "next/app";
import { Fragment, useMemo } from "react";
import Layout from "../components/Layout";

function MyApp({ Component, pageProps }: AppProps) {
  const content = useMemo(() => {
    return (
      <Layout>
        <Component {...pageProps} />
      </Layout>
    );
  }, [Component, pageProps]);
  return <Fragment>{content}</Fragment>;
}

export default MyApp;
