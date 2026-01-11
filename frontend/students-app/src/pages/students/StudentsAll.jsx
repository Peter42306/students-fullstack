import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { deleteStudent, getStudents } from "../../api/studentsApi";
import StudentTable from "../../components/students/StudentTable";
import StudentsEmptyTable from "../../components/students/StudentsEmptyTable";
import StudentsError from "../../components/students/StudentsError";
import StudentsLoading from "../../components/students/StudentsLoading";
import StudentSearchBar from "../../components/students/StudentSearchBar";
import PaginationBar from "../../components/students/PaginationBar";
import PaginationBarRange from "../../components/students/PaginationBar2";

export default function StudentsAll() {

    const [students, setStudents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const [search, setSearch] = useState('');
    const [debouncedSearch, setDebouncedSearch] = useState('');

    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalCount, setTotalCount] = useState(0);

    const [sortBy, setSortBy] = useState('id');
    const [sortDirection, setSortDirection] = useState('desc');

    const totalPages = Math.max(1, Math.ceil(totalCount / pageSize));

    async function load(searchValue = debouncedSearch, pageValue = page) {
        setLoading(true);
        setError('');

        try {
            const data = await getStudents(searchValue, pageValue, pageSize, sortBy, sortDirection);
            setStudents(data.items);
            setTotalCount(data.totalCount);
        } catch (err) {
            setError(err?.message ?? 'Unknown error')
        } finally {
            setLoading(false);
        }
    }

    useEffect(() => {
        const id = setTimeout(() => {
            setDebouncedSearch(search);
        }, 1000);

        return () => clearTimeout(id);
    }, [search]);

    useEffect(() => {
        setPage(1);
    },[debouncedSearch]);

    useEffect(() => {
        load();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    },[debouncedSearch, page, pageSize, sortBy, sortDirection]);
    

    async function onDelete(id) {
        if(!confirm('Delete student?')){
            return;
        }

        try {
            await deleteStudent(id)
            await load();
        } catch (err) {
            alert(err?.message ?? 'Delete failed');
        }
    }

    function handleSearchNow() {
        const trimmed = search.trim();
        setDebouncedSearch(trimmed);
        setPage(1);                
        load(trimmed, 1);
    }

    function onSort(col){
        setPage(1);

        if(sortBy === col){
            setSortDirection(d => (d === 'asc' ? 'desc' : 'asc'));
        } else {
            setSortBy(col);
            setSortDirection('asc');
        }
    }

    return(
        <div className="container py-3">

            <div className="d-flex justify-content-between align-items-center">
                <h1 className="mb-0">Students</h1>            
                <div className="text-end">
                <Link 
                    to="/students/create" 
                    className="btn btn-outline-primary btn-fixed"                    
                >
                    Add
                </Link>
            </div>

            </div>
            
            <hr/>           

            <div className="d-flex flex-column gap-2">
                <StudentSearchBar 
                    search={search} 
                    onSearchChange={setSearch} 
                    onSearch={handleSearchNow}
                />
                {/* {loading && <StudentsLoading/>} */}
                {error && <StudentsError error={error}/>}
                
                {!loading && !error && students.length === 0 &&<StudentsEmptyTable/>}

                {!loading && !error && students.length > 0 &&
                <StudentTable   
                    students={students} 
                    onDelete={onDelete}
                    sortBy={sortBy}
                    sortDirection={sortDirection}
                    onSort={onSort}
                />}

                <div className="d-flex align-items-center justify-content-end gap-2">
                    <small className="text-nowrap">
                        <span className="d-inline d-sm-none">Rows: </span>
                        <span className="d-none d-sm-inline">Rows per page: </span>                        
                    </small>
                    <select
                        className="form-select form-select-sm w-auto"
                        value={pageSize}
                        onChange={(e) => {
                            setPageSize(Number(e.target.value));
                            setPage(1)
                        }}
                    >
                        <option value={5}>5</option>
                        <option value={10}>10</option>
                        <option value={20}>20</option>
                        <option value={30}>30</option>
                    </select>
                    
                    <PaginationBarRange
                        page={page}
                        pageSize={pageSize}
                        totalCount={totalCount}
                        totalPages={totalPages}
                        onPageChange={setPage}
                    />
                </div>
                
            </div>

        </div>
    );
}