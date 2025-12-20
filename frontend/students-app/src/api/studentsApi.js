const API_BASE = '/api/students';

async function handleResponse(res) {
    if(!res.ok){
        const text = await res.text().catch(() => '');
        throw new Error(`API error ${res.status}: ${text || res.statusText}`);
    }
    if(res.status ===204){
        return;
    }
    return res.json();
}

// /api/Students
export async function getStudents(search) {
    const url = search 
        ? `${API_BASE}?search=${encodeURIComponent(search)}` 
        : API_BASE;
    const res = await fetch(url);
    return handleResponse(res);
}

// /api/Students
export async function createStudent(dto) {
    const res = await fetch(API_BASE,{
        method:'POST',
        headers:{'Content-Type': 'application/json'},
        body:JSON.stringify(dto),
    });
    return handleResponse(res);
}

// /api/Students/{id}
export async function deleteStudent(id) {
    const res = await fetch(`${API_BASE}/${id}`,{method: 'DELETE'});
    if(res.status === 204){
        return;
    }
    return handleResponse(res);
}

// /api/Students/{id}
export async function getStudent(id) {
    const res = await fetch(`${API_BASE}/${id}`);
    return handleResponse(res);
}

// /api/Students/{id}
export async function updateStudent(id, dto) {
  const res = await fetch(`${API_BASE}/${id}`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(dto),
  });
  return handleResponse(res);
}