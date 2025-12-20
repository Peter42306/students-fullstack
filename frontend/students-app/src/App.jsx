import { Navigate, Route, Routes } from 'react-router-dom'
import './App.css'
import StudentCreate from './pages/students/StudentCreate'
import StudentDetails from './pages/students/StudentDetails'
import StudentsAll from './pages/students/StudentsAll'
import StudentUpdate from './pages/students/StudentUpdate'


function App() {
  

  return (
    <Routes>
      <Route path='/' element={<Navigate to="/students" replace/>}/>
      <Route path='/students' element={<StudentsAll/>}/>
      <Route path='/students/create' element={<StudentCreate/>}/>
      <Route path='/students/:id' element={<StudentDetails/>}/>
      <Route path='/students/:id/edit' element={<StudentUpdate/>}/>
    </Routes>    
  )
}

export default App
