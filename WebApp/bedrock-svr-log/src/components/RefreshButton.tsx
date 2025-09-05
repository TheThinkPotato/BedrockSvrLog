import { Box } from "@mui/material"

interface RefreshButtonProps {
    refreshIframe: () => void;
    showSeedMap: boolean;
}

const RefreshButton = ({ refreshIframe, showSeedMap }: RefreshButtonProps) => {
    return (
        <Box
          className="fixed z-50"
          style={{
            ...(!showSeedMap
              ? { left: "0.5rem", top: "4.5rem" }
              : { left: "14rem", top: "6.5rem" }),
                      
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
    )
}   

export default RefreshButton;