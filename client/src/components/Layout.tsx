import { ReactChild } from "react";

const Layout: React.FC = ({ children }) => {
  return (
    <div className="layout">
      <div className="layout_header">
        <div style={{ height: "200px" }}></div>
      </div>
      <div className="layout_body">
        <div style={{ height: "1000px" }}>{children}</div>
      </div>
      <div className="layout_footer">
        <div style={{ height: "100px" }}></div>
      </div>
    </div>
  );
};
export default Layout;
