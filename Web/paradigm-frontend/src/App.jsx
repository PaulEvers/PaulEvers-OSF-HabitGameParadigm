import "./App.css";
import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Day1 from "./components/Day1";
import Day2 from "./components/Day2";
import Day3 from "./components/Day3";
import Overview from "./components/Overview";
function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Overview />} />
        <Route path="/1af8d72c" element={<Overview />} />
        <Route path="/1af8d72c/day1" element={<Day1 />} />
        <Route path="/1af8d72c/day2" element={<Day2 />} />
        <Route path="/1af8d72c/day3" element={<Day3 />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
