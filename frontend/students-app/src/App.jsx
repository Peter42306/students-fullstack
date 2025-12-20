import { Navigate, Route, Routes } from 'react-router-dom'
import './App.css'
import StudentCreate from './pages/students/StudentCreate'
import StudentDetails from './pages/students/StudentDetails'
import StudentsAll from './pages/students/StudentsAll'


function App() {
  

  return (
    <Routes>
      <Route path='/' element={<Navigate to="/students" replace/>}/>
      <Route path='/students' element={<StudentsAll/>}/>
      <Route path='/students/create' element={<StudentCreate/>}/>
      <Route path='/students/details' element={<StudentDetails/>}/>
    </Routes>    
  )
}

export default App
