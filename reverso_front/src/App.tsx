
import './App.css';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';

function App() {
  return (
      <Router>
        <Routes>
          <Route path="/game" element={<GamePage />} />
          <Route path="/waiting" element={<WaitingPage />} />
          <Route path="/profile" element={<ProfilePage />} />
          <Route path="/" element={<LoginPage />} />
        </Routes>
      </Router>
  );
}


export default App;
