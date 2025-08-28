import { Box } from "@mui/material";
import { useRef } from "react";
import WorldClock from "./WorldClock";

const HomeScreen = () => {
  const iframeRef = useRef<HTMLIFrameElement>(null);

  const refreshIframe = () => {
    if (iframeRef.current) {
      const currentSrc = iframeRef.current.src;
      iframeRef.current.src = "";
      setTimeout(() => {
        if (iframeRef.current) {
          iframeRef.current.src = currentSrc;
        }
      }, 100);
    }
  };

  return (
    <Box className="w-full h-full p-4">
      <Box className="w-full h-full rounded-lg overflow-hidden border border-gray-700">
        <WorldClock />

        <Box
          className="fixed z-50"
          style={{
            left: "0.5rem",
            top: "4.5rem",
            backgroundColor: "rgba(255, 255, 255, 1)",
            minWidth: "14px",
            height: "33px",
            borderRadius: "0.2rem",
            boxShadow: "3px 3px 2px rgba(0, 0, 0, 0.2)",
            color: "black",
            fontFamily:
              "-apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Helvetica, Arial, sans-serif",
            cursor: "pointer",
            fontSize: "16px",
          }}
          onClick={refreshIframe}
        >
          <Box className="flex flex-col gap-2 overflow-y-auto max-h-[200px]">
            <Box
              style={{
                fontSize: "16px",
                paddingBlock: "0.2rem",
                paddingInline: "0.5rem",
              }}
            >
              {`↻`}
            </Box>
          </Box>
        </Box>

        <iframe
          ref={iframeRef}
          src="map/index.html"
          title="Bedrock Server Map"
          className="w-full h-full border-0"
          sandbox="allow-same-origin allow-scripts allow-forms allow-popups allow-popups-to-escape-sandbox"
        />
      </Box>
    </Box>
  );
};

export default HomeScreen;
