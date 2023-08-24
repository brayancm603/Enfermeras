/* eslint-disable react/jsx-no-undef */
import Cruz from '../../public/images/cruzRoja.jpg'
import Image from 'next/image'

export default function Home() {
  return (
    <div className="bg-[#DEF5E5] h-full w-full">
      <div>
        <div className='p-5 text-black'>
          <h3>
            Bienvenidos a Virtual Nurse, nuestro objetivo es ayudarlo en cuanto
            a su salud procurando su vienestar, brindamos servicios a domicilio
            con personla calificado y de acuerdo al tipo de emergencia.
          </h3>
        </div>
        <div className='h-[20vh] flex items-center justify-center'>
          <button class="bg-[#8EC3B0] hover:bg-green-200 text-black font-bold py-2 px-4 rounded-full">
            INFORMACION
          </button>
          <button class="ml-5 bg-[#8EC3B0] hover:bg-green-200 text-black font-bold py-2 px-4 rounded-full">
            COMENZAR
          </button>  
        </div>
        <div className='flex justify-center items-center'>
          <Image className='bg-white rounded-full' src={Cruz} width={'19%'} height={'auto'} alt="" />
        </div>
      </div>
    </div>
  )
}
