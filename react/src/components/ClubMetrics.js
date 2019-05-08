import React, { Component } from "react";
import {
  Bar,
  CartesianGrid,
  ComposedChart,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis
} from "recharts";

class ClubMetrics extends Component {
  render() {
    const { metrics, isLoading } = this.props;
    if (!isLoading && metrics.length) {
      return (
        <div className="mx-auto text-center">
          <h2 className="text-center">Previous Years Membership Count</h2>
          <ResponsiveContainer width="100%" height={200}>
            <ComposedChart
              data={metrics}
              margin={{ top: 20, right: 30, left: 20, bottom: 0 }}
              barGap={5}
              barCategoryGap={10}
            >
              <YAxis type="number" allowDataOverflow={false} hide />
              <XAxis dataKey="date" type="category" />
              <Tooltip />
              <CartesianGrid stroke="#f5f5f5" />
              <Bar
                dataKey="members"
                barSize={15}
                fill="#f44242"
                label={{ position: "top", offset: 7 }}
              />
            </ComposedChart>
          </ResponsiveContainer>
          <br />
          <h2 className="text-center">Number of Goals Achieved</h2>
          <ResponsiveContainer width="100%" height={200}>
            <ComposedChart
              data={metrics}
              margin={{ top: 20, right: 30, left: 20, bottom: 0 }}
              barGap={5}
              barCategoryGap={10}
            >
              <YAxis type="number" allowDataOverflow={false} hide />
              <XAxis dataKey="date" type="category" />
              <Tooltip />
              <CartesianGrid stroke="#f5f5f5" />
              <Bar
                dataKey="goals"
                barSize={15}
                fill="#f44242"
                label={{ position: "top", offset: 7 }}
              />
            </ComposedChart>
          </ResponsiveContainer>
        </div>
      );
    } else if (!isLoading) {
      return (
        <div className="mx-auto text-center">
          <h2 className="text-center">Club Metrics Could Not Be Found</h2>
        </div>
      );
    }
    return null;
  }
}

export default ClubMetrics;
